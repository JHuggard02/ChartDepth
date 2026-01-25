using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ChartDepth.Models
{
    class TickerInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static int numSteps = 10;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TickerInfo()
        {
            ChartBidQuantities = new double[10] { 400, 300, 200, 100, 0, 0, 0, 0, 0, 0 };
            ChartAskQuantities = new double[10] { 0, 0, 0, 0, 100, 200, 300, 200, 100, 100 };
            ChartPrices = new double[10] { 11.1, 11.2, 11.3, 11.4, 11.5, 11.6, 11.7, 11.8, 11.9, 20};

            updateRectangles();
        }

        double MaxQuantity = 0;

        double[] ChartBidQuantities = new double[numSteps];
        double[] ChartAskQuantities = new double[numSteps];
        double[] ChartPrices = new double[numSteps];

        public ObservableCollection<RectangleModel> Rectangles {get; set;}

        public ObservableCollection<RectangleModel> BidRectangles { get; set; }
        public ObservableCollection<RectangleModel> AskRectangles { get; set; }
        public ObservableCollection<ChartPriceModel> ChartPricesO { get; set; }

        public void updateTotalQuantity()
        {
            MaxQuantity = 0;
            foreach(double quantity in ChartBidQuantities)
            {
                MaxQuantity = Math.Max(MaxQuantity, quantity);
            }
            foreach(double quantity in ChartAskQuantities)
            {
                MaxQuantity = Math.Max(MaxQuantity, quantity);
            }
        }
        public void updateRectangles()
        {
            updateTotalQuantity();
            BidRectangles = new ObservableCollection<RectangleModel>();
            AskRectangles = new ObservableCollection<RectangleModel>();
            ChartPricesO = new ObservableCollection<ChartPriceModel>();
            for (int i = 0; i < numSteps; ++i)
            {
                if (ChartBidQuantities[i] == 0)
                {
                    continue;
                }
                BidRectangles.Add(new RectangleModel(
                    (1 - ((double)(i + 1) / numSteps)),
                    ChartBidQuantities[i] / MaxQuantity,
                    true,
                    (double)1 / numSteps));
            }

            for (int i = 0; i < numSteps; ++i)
            {
                if (ChartAskQuantities[i] == 0)
                {
                    continue;
                }
                AskRectangles.Add(new RectangleModel(
                    (1 - ((double)(i + 1) / numSteps)),
                    ChartAskQuantities[i] / MaxQuantity,
                    false,
                    (double)1 / numSteps));
            }

            for (int i = 0; i < numSteps; ++i)
            {
                ChartPricesO.Add(new ChartPriceModel(ChartPrices[i], (1 - ((double)(i + 1) / numSteps))));
            }

        }
        
    }
}
