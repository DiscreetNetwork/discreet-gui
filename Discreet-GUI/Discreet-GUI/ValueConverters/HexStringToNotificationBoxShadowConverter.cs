using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class HexStringToNotificationBoxShadowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string) return value;
            string v = (string)value;

            BoxShadows b = new(new BoxShadow { OffsetX = 0, OffsetY = 0, Blur = 15, Spread = -2, Color = Color.Parse(v) });
            return b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
