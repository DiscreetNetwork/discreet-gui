using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class StringNotEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;

            return !string.IsNullOrEmpty(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
