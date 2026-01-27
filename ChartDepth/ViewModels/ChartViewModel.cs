using ChartDepth.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChartDepth.ViewModels
{
    class ChartViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<PriceLevel> _bids;
        private ObservableCollection<PriceLevel> _asks;


        private string _title;
        private double _rectWidth;
        private double _rectangleHeight;
        private ObservableCollection<RectangleModel> _rectangles;
        private ObservableCollection<RectangleModel> _bidRectangles;
        private ObservableCollection<RectangleModel> _askRectangles;
        private ObservableCollection<ChartPriceModel> _chartPrices;
        private ObservableCollection<InfoRows> _verticalBook;

        private TickerInfo _tickerInfo;

        public ChartViewModel()
        {
            _tickerInfo = new TickerInfo();
            Title = "My View Model";
            RectWidth = 0.4;

            Rectangles = new ObservableCollection<RectangleModel>();
            Rectangles.Add(new RectangleModel(0.1, 0.75, true, 0.1));
            Rectangles.Add(new RectangleModel(0.2, 0.5, true, 0.1));
            Rectangles.Add(new RectangleModel(0.3, 0.3, true, 0.1));

            BidRectangles = _tickerInfo.BidRectangles;
            AskRectangles = _tickerInfo.AskRectangles;
            ChartPrices = _tickerInfo.ChartPricesO;

            RectangleHeight = 0.1;
                            
            OnPropertyChanged(nameof(Rectangles));
            OnPropertyChanged(nameof(BidRectangles));
            OnPropertyChanged(nameof(AskRectangles));

            _bids = new ObservableCollection<PriceLevel>();
            _asks = new ObservableCollection<PriceLevel>();

            _bids.Add(new PriceLevel(12.4, 100));
            _bids.Add(new PriceLevel(12.6, 200));
            _bids.Add(new PriceLevel(12.8, 300));

            _asks.Add(new PriceLevel(13.1, 200));
            _asks.Add(new PriceLevel(13.4, 300));
            _asks.Add(new PriceLevel(13.7, 500));

            OnPropertyChanged(nameof(Bids));
            OnPropertyChanged(nameof(Asks));

            _verticalBook = new ObservableCollection<InfoRows>();

            _verticalBook.Add(new InfoRows(11.7, 10, 0));
            _verticalBook.Add(new InfoRows(11.6, 10, 0));
            _verticalBook.Add(new InfoRows(11.5, 10, 0));
            _verticalBook.Add(new InfoRows(11.4, 0, 0));
            _verticalBook.Add(new InfoRows(11.3, 0, 0));
            _verticalBook.Add(new InfoRows(11.2, 0, 10));
            _verticalBook.Add(new InfoRows(11.1, 0, 10));
            _verticalBook.Add(new InfoRows(11.0, 0, 10));

            OnPropertyChanged(nameof(VerticalBook));

        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public double RectWidth
        {
            get => _rectWidth;
            set
            {
                if (_rectWidth != value)
                {
                    _rectWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        public double RectangleHeight
        {
            get => _rectangleHeight;
            set
            {
                if (_rectangleHeight != value)
                {
                    _rectangleHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<RectangleModel> Rectangles
        {
            get => _rectangles;
            set
            {
                if (_rectangles != value)
                {
                    _rectangles = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<RectangleModel> BidRectangles
        {
            get => _bidRectangles;
            set
            {
                if (_bidRectangles != value)
                {
                    _bidRectangles = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<RectangleModel> AskRectangles
        {
            get => _askRectangles;
            set
            {
                if (_askRectangles != value)
                {
                    _askRectangles = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ChartPriceModel> ChartPrices
        {
            get => _chartPrices;
            set
            {
                if (_chartPrices != value)
                {
                    _chartPrices = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<PriceLevel> Bids
        {
            get { return _bids; }
        }

        public ObservableCollection<PriceLevel> Asks
        {
            get { return _asks; }
        }

        public ObservableCollection<InfoRows> VerticalBook
        {
            get { return _verticalBook; }
        }
    }
}
