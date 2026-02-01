using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    public class Candle
    {
        public double HighHorizontalLineOffset { get; set; }
        public double BodyTopOffset { get; set; }
        public double CandleBodyHeight { get; set; }
        public double BodyBottomOffset { get; set; }
        public double LowHorizontalLineOffset { get; set; }
        public bool IsBull { get; set; }
        public double CandleWidth { get; set; }
        public double LeftHorizontalOffset { get; set; }
        public double MiddleHorizontalOffset { get; set; }
        public double RightHorizontalOffset { get; set; }

        public Candle (double Open, double High, double Low, double Close, double MaxHigh, double MinLow, double candleWidth, double leftOffset, double MiddleOffset, double RightOffset)
        {
            CandleWidth = 0.16;
            double scale = MaxHigh - MinLow;
            IsBull = (Close > Open) ? true : false;
            HighHorizontalLineOffset = (1 - (High - MinLow) / scale);
            BodyTopOffset = (IsBull) ? (1 - (Close - MinLow) / scale) : (1 - (Open - MinLow) / scale);
            CandleBodyHeight = (IsBull) ? (Close - Open) / scale : (Open - Close) / scale;
            BodyBottomOffset = (IsBull) ? (1 - (Open - MinLow) / scale) : (1 - (Close - MinLow) / scale);
            LowHorizontalLineOffset = (1 - (Low - MinLow) / scale);
            CandleWidth = candleWidth;
            LeftHorizontalOffset = leftOffset;
            MiddleHorizontalOffset = MiddleOffset;
            RightHorizontalOffset = RightOffset;
        }
    }
}
