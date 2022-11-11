using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class FullAddressToTruncatedAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fullAddress = (string)value;
            return $"{fullAddress.Substring(0, 10)}...{fullAddress.Substring(fullAddress.Length - 10, 10)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
