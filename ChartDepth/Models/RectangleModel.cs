using System;
using System.Collections.Generic;
using System.Text;

namespace ChartDepth.Models
{
    public class RectangleModel
    {
        public double Offset { get; set; }
        public double Quantity { get; set; }
        public bool IsBid { get; set; }
        public double Height { get; set; }

        public RectangleModel(double offset, double quantity, bool isBid, double height)
        {
            Offset = offset;
            Quantity = quantity;
            IsBid = isBid;
            Height = height;

        }
    }
}
