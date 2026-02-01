using ChartDepth.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace ChartDepth.Services
{
    public class MarketDataService
    {
        private Dictionary<uint, object> _snapshotLockByTickerId;
        private Dictionary<uint, List<Snapshot>> _snapshotByTickerId;
        private double _candleStep = (double)1 / 5;
        private double _candleBuffer;
        public MarketDataService()
        {
            _snapshotByTickerId = new Dictionary<uint, List<Snapshot>>();
            _snapshotLockByTickerId = new Dictionary<uint, object>();
            string dateTime = new DateTime(2025, 4, 25, 5, 7, 8, 730, DateTimeKind.Local).ToString("h: mm");
            Snapshot snapshot = new Snapshot(1, dateTime, 20);
            snapshot.Open = 20;
            snapshot.High = 25;
            snapshot.Low = 10;
            snapshot.Close = 15;
            List<Snapshot> snapshotList = new List<Snapshot>() { snapshot, snapshot, snapshot, snapshot, snapshot };
            _snapshotByTickerId[1] = snapshotList;
            _snapshotLockByTickerId[1] = new object();
            _candleBuffer = _candleStep * 0.25;
        }
        public void UpdateSnapshot(OrderBookOrder order)
        {
            // Lock, update last element in the list
            object _snapshotLock = _snapshotLockByTickerId[order.TickerId];
            lock(_snapshotLock)
            {
                List<Snapshot> _snapshots = _snapshotByTickerId[order.TickerId];
                Snapshot currSnapshot = _snapshots.Last();
                currSnapshot.Update(order.Price);
            }
        }

        public void NewSnapshot(uint tickerId, string time)
        {
            // Lock, Create new empty Snapshot, append
            object _snapshotLock = _snapshotLockByTickerId[tickerId];
            lock (_snapshotLock)
            {
                List<Snapshot> _snapshots = _snapshotByTickerId[tickerId];
                Snapshot lastSnapshot = _snapshots.Last();
                _snapshots.Add(new Snapshot(tickerId, time, lastSnapshot.Close));
            } 
        }

        public ObservableCollection<Candle> GetSnapshot(uint tickerId)
        {
            List<Snapshot> copy;

            lock (_snapshotLockByTickerId[tickerId])
            {
                copy = _snapshotByTickerId[tickerId].ToList(); 
            }

            var lastFive = copy.TakeLast(5).ToList();

            double maxHigh = lastFive.Max(s => s.High) * 1.1;
            double minLow = lastFive.Min(s => s.Low) * 0.9;

            var candles = new ObservableCollection<Candle>();
            double candleWidth = (_candleStep - ((double)2 * _candleBuffer));
            for (int i = 0; i < 5; i++)
            {
                var snapshot = lastFive[i];
                double leftOffset = (_candleStep * i) + (_candleBuffer);
                double middleOffset = (_candleStep * (i + 0.5));
                double rightOffset = (_candleStep * (i + 1)) - (_candleBuffer);
                candles.Add(new Candle(snapshot.Open, snapshot.High, snapshot.Low, snapshot.Close, maxHigh, minLow, candleWidth, leftOffset, middleOffset, rightOffset));
            }

            return candles;
        }

    }
}

