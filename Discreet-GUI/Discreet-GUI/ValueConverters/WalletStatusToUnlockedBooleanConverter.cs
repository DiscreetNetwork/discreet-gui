using Avalonia.Data.Converters;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discreet_GUI.ValueConverters
{
    /// <summary>
    /// The given 'WalletStatus' indicates whether or not the wallet is unlocked
    /// </summary>
    public class WalletStatusToUnlockedBooleanConverter : IValueConverter
    {
        enum Parameters
        {
            Normal, Inverted
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WalletStatus status = (WalletStatus)value;
            var direction = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);


            switch (status)
            {
                case WalletStatus.UNLOCKED:
                    return direction == Parameters.Normal ? true : false;
                case WalletStatus.LOCKED:
                    return direction == Parameters.Normal ? false : true;
                case WalletStatus.UNLOADED:
                    return direction == Parameters.Normal ? false : true;
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
