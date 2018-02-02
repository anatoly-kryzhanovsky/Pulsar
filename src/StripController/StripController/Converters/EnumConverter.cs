using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace StripController.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            foreach (var one in Enum.GetValues(parameter as Type))
            {
                if (value.Equals(one))
                    return GetDescription(one);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            foreach (var one in Enum.GetValues(parameter as Type))
            {
                if (value.ToString() == GetDescription(one))
                    return one;
            }

            return null;
        }

        private string GetDescription(object item)
        {
            var type = item.GetType();
            var memberInfo = type.GetMember(item.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var value = ((DescriptionAttribute)attributes[0]).Description;

            return value;
        }
    }
}