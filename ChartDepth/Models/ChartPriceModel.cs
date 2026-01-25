using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    public class ChartPriceModel
    {
        public double Price { get; set; }
        public double Offset { get; set; }

        public ChartPriceModel(double price, double offset)
        {
            Price = price;
            Offset = offset;
        }
    }
}
