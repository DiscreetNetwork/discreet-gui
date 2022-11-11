using Avalonia.Data.Converters;
using Services.Testnet;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class IssueSeverityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int severityValue = (int)value;
            return (IssueSeverity)severityValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IssueSeverity severityValue = (IssueSeverity)value;
            return (int)severityValue;
        }
    }
}
