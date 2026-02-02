using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MarketMaker
    {
        // Base spread in basis points (e.g., 10 = 0.1%)
        private readonly double _baseSpreadBps;
        // How much to widen spread based on volatility
        private readonly double _volatilityMultiplier;
        // How much inventory affects quote skew
        private readonly double _inventorySkewFactor;
        // Maximum inventory position
        private readonly double _maxInventory;
        // Base size per quote level
        private readonly double _quoteSize;
        // Number of levels to quote on each side
        private readonly int _quoteLevels;
        // Spacing between quote levels in bps
        private readonly double _levelSpacingBps;
        // Directional edge adjustment (positive = bullish bias)
        private readonly double _alphaAdjustment;
        // Number of trades to consider for volatility
        private readonly int _lookbackPeriod;             

        public double CurrentInventory { get; private set; }
        public double MidPrice { get; private set; }
        public double BestBid { get; private set; }
        public double BestAsk { get; private set; }
        public double Spread { get; private set; }
        public double Skew { get; private set; }
        public double EstimatedVolatility { get; private set; }
        public DateTime LastUpdateTime { get; private set; }
        public bool IsStale { get; private set; }

        public MarketMaker(
            double baseSpreadBps = 10.0,
            double volatilityMultiplier = 2.0,
            double inventorySkewFactor = 0.5,
            double maxInventory = 100000.0,
            double quoteSize = 1000.0,
            int quoteLevels = 3,
            double levelSpacingBps = 5.0,
            double alphaAdjustment = 0.0,
            int lookbackPeriod = 20)
        {
            _baseSpreadBps = baseSpreadBps;
            _volatilityMultiplier = volatilityMultiplier;
            _inventorySkewFactor = inventorySkewFactor;
            _maxInventory = maxInventory;
            _quoteSize = quoteSize;
            _quoteLevels = quoteLevels;
            _levelSpacingBps = levelSpacingBps;
            _alphaAdjustment = alphaAdjustment;
            _lookbackPeriod = lookbackPeriod;

            CurrentInventory = 0.0;
            LastUpdateTime = DateTime.UtcNow;
        }

        public List<(double price, ulong volume)> Quote(Queue<(double price, ulong volume)> pastTrades, SortedDictionary<double, ulong> bids,   SortedDictionary<double, ulong> asks)  
        {
            LastUpdateTime = DateTime.UtcNow;

            UpdateMarketState(bids, asks);

            IsStale = pastTrades.Count == 0 || bids.Count == 0 || asks.Count == 0;
            if (IsStale)
            {
                // Don't quote in stale markets
                return new List<(double, ulong)>(); 
            }

            EstimatedVolatility = CalculateVolatility(pastTrades);


            double dynamicSpreadBps = _baseSpreadBps + (EstimatedVolatility * _volatilityMultiplier);
            double halfSpread = MidPrice * (dynamicSpreadBps / 20000.0); // Divide by 20000 to get half-spread

            // Calculate inventory skew (negative inventory = long, want higher quotes to sell)
            double inventoryRatio = CurrentInventory / _maxInventory;
            Skew = inventoryRatio * _inventorySkewFactor * halfSpread;

            // Apply alpha adjustment (directional edge)
            double alphaShift = MidPrice * (_alphaAdjustment / 10000.0);

            // Generate quotes
            var quotes = new List<(double price, ulong volume)>();

            // Calculate adjusted mid price
            double adjustedMid = MidPrice - Skew + alphaShift;

            // bid quotes
            for (int i = 0; i < _quoteLevels; i++)
            {
                double levelOffset = MidPrice * ((dynamicSpreadBps + i * _levelSpacingBps) / 10000.0);
                double bidPrice = adjustedMid - halfSpread - (i * MidPrice * _levelSpacingBps / 10000.0);

                if (bidPrice >= BestAsk || bidPrice > BestBid)
                    continue;

                // adjust for inventory
                double sizeMultiplier = 1.0 - (Math.Max(0, inventoryRatio) * 0.5);
                ulong bidSize = (ulong)Math.Max(100, _quoteSize * sizeMultiplier);

                quotes.Add((Math.Round(bidPrice, 2), bidSize));
            }

            // ask quotes
            for (int i = 0; i < _quoteLevels; i++)
            {
                double askPrice = adjustedMid + halfSpread + (i * MidPrice * _levelSpacingBps / 10000.0);

                if (askPrice <= BestBid || askPrice < BestAsk)
                    continue;

                double sizeMultiplier = 1.0 + (Math.Min(0, inventoryRatio) * 0.5);
                ulong askSize = (ulong)Math.Max(100, _quoteSize * sizeMultiplier);

                quotes.Add((Math.Round(askPrice, 2), askSize));
            }

            return quotes;
        }

        private void UpdateMarketState(
            SortedDictionary<double, ulong> bids,
            SortedDictionary<double, ulong> asks)
        {
            BestBid = bids.Count > 0 ? bids.Keys.Max() : 0;
            BestAsk = asks.Count > 0 ? asks.Keys.Min() : 0;

            if (BestBid > 0 && BestAsk > 0)
            {
                MidPrice = (BestBid + BestAsk) / 2.0;
                Spread = BestAsk - BestBid;
            }
        }

        private double CalculateVolatility(Queue<(double price, ulong volume)> pastTrades)
        {
            if (pastTrades.Count < 2)
                return 0.01; 

            var recentTrades = pastTrades.TakeLast(Math.Min(_lookbackPeriod, pastTrades.Count)).ToList();

            if (recentTrades.Count < 2)
                return 0.01;

            // Calculate log returns
            var returns = new List<double>();
            for (int i = 1; i < recentTrades.Count; i++)
            {
                if (recentTrades[i - 1].price > 0 && recentTrades[i].price > 0)
                {
                    double logReturn = Math.Log(recentTrades[i].price / recentTrades[i - 1].price);
                    returns.Add(logReturn);
                }
            }

            if (returns.Count == 0)
                return 0.01;

            // Calculate standard deviation of returns
            double mean = returns.Average();
            double variance = returns.Sum(r => Math.Pow(r - mean, 2)) / returns.Count;
            double stdDev = Math.Sqrt(variance);

            // Annualize and convert to percentage (assuming trades are per minute)
            // Adjust multiplier based on your trade frequency
            double annualizedVol = stdDev * Math.Sqrt(525600); // Minutes in a year

            return Math.Max(0.001, Math.Min(1.0, annualizedVol)); // Clamp between 0.1% and 100%
        }

        // Method to update inventory after trades
        public void UpdateInventory(double tradePrice, long tradeVolume)
        {
            // Positive volume = bought (long), negative = sold (short)
            CurrentInventory += tradeVolume;
            CurrentInventory = Math.Max(-_maxInventory, Math.Min(_maxInventory, CurrentInventory));
        }
    }
}
