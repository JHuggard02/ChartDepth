using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ChartDepth.Views
{
    // Converter for coloring PNL values (positive = green, negative = red, zero = gray)
    public class PnlColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double pnl)
            {
                if (pnl > 0)
                    return new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Green
                else if (pnl < 0)
                    return new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Red
                else
                    return new SolidColorBrush(Color.FromRgb(100, 116, 139)); // Gray
            }

            return new SolidColorBrush(Color.FromRgb(203, 213, 225)); // Default gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter to show error pill only when error exists
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}