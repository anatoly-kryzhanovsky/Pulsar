using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StripController.Converters
{
    public class ColorComponentToColorConverter : IMultiValueConverter
    {
        private static readonly Color DefaultColor = Color.FromRgb(0, 0, 0);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 3)
                return DefaultColor;

            if (!(values[0] is byte) || !(values[1] is byte) || !(values[2] is byte))
                return DefaultColor;

            var r = (byte)values[0];
            var g = (byte)values[1];
            var b = (byte)values[2];

            return Color.FromRgb(r, g, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var color = (Color) value;

            return new object[] {color.R, color.G, color.B};
        }
    }
}