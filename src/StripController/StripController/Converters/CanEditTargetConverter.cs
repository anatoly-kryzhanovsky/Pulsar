using System;
using System.Globalization;
using System.Windows.Data;
using StripController.PresentationEntities;

namespace StripController.Converters
{
    public class CanEditTargetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (EProgramItemType) value;
            return type == EProgramItemType.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}