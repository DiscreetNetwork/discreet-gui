using Avalonia.Data.Converters;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ValueConverters
{
    /// <summary>
    /// The given 'WalletStatus' indicates whether or not the wallet is unlocked
    /// </summary>
    public class WalletStatusToUnlockedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WalletStatus status = (WalletStatus)value;

            switch (status)
            {
                case WalletStatus.UNLOCKED:
                    return "Continue";
                case WalletStatus.LOCKED:
                    return "Unlock";
                case WalletStatus.UNLOADED:
                    return "Unlock";
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
