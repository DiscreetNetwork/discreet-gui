using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class StringToULongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (value is string) return (string)value;

            if (value is long && (long)value == 0) return string.Empty;

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((long)value).ToString();
        }
    }
}
