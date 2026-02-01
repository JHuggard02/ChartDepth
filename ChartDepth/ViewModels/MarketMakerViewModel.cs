using ChartDepth.Models;
using ChartDepth.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChartDepth.ViewModels
{
    public class MarketMakerViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private MarketDataService _marketDataService = new MarketDataService();

        private ObservableCollection<Candle> _candles;

        public MarketMakerViewModel()
        {
            Candles = _marketDataService.GetSnapshot(1);
        }

        public ObservableCollection<Candle> Candles
        {
            get { return _candles; }
            set
            {
                if (_candles != value)
                {
                    _candles = value;
                    OnPropertyChanged(nameof(Candles));
                }
            }
        }

    }
}
