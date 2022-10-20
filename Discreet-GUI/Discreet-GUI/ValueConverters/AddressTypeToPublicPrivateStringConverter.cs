using Avalonia.Data.Converters;
using Services.Caches;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discreet_GUI.ValueConverters
{
    public class AddressTypeToPublicPrivateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WalletCache.AddressType type = (WalletCache.AddressType)value;
            switch(type)
            {
                case WalletCache.AddressType.STEALTH:
                    return "Private";

                case WalletCache.AddressType.TRANSPARENT:
                    return "Public";

                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value.ToString().ToLower())
            {
                case "private":
                    return WalletCache.AddressType.STEALTH;

                case "public":
                    return WalletCache.AddressType.TRANSPARENT;

                default:
                    return value;
            }
        }
    }
}
