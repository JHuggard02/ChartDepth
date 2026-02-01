using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    internal class Snapshot
    {
        public uint TickerId { get; set; }
        public string StartTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public bool initialised = false;

        public Snapshot(uint id, string startTime, double marketPrice)
        {
            TickerId = id;
            StartTime = startTime;
            Open = marketPrice;
            High = marketPrice;
            Low = marketPrice;
            Close = marketPrice;
        }

        public void Update(double currPrice)
        {
            Low = Math.Min(Low, currPrice);
            High = Math.Max(High, currPrice);
            Close = currPrice;
        }

    }
}
