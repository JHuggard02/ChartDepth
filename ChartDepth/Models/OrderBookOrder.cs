using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    public struct OrderBookOrder
    {
        public uint TickerId;
        public double Price;
        public ulong Volume;
        public bool IsTrade;
    }
}
