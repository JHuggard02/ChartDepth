using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    public class InfoRows
    {
        public double Price { get; set; }
        public ulong AskQuantity { get; set; }
        public ulong BidQuantity { get; set; }

        public InfoRows(double price, ulong askQuantity, ulong bidQuantity)
        {
            Price = price;
            AskQuantity = askQuantity;
            BidQuantity = bidQuantity;
        }
    }
}
