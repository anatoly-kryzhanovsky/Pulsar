using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StripController.Converters
{
    public class BrightnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brightnes = (byte) value;
            return new SolidColorBrush(Color.FromRgb(brightnes, brightnes, brightnes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}