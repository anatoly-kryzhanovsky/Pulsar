using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripController.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility VisibleForTrue { get; set; }
        public Visibility VisibleForFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return VisibleForFalse;

            var isVisible = (bool) value;
            if (isVisible)
                return VisibleForTrue;

            return VisibleForFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}