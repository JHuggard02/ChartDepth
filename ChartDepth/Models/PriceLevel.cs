using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    public class PriceLevel
    {
        public double Price { get; set; }
        public ulong Volume { get; set; }

        public PriceLevel(double price, ulong volume)
        {
            Price = price;
            Volume = volume;
        }
    }
}
