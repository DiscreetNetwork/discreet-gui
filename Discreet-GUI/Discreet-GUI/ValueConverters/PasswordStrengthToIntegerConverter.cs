using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Discreet_GUI.Utility;

namespace Discreet_GUI.ValueConverters
{
    public class PasswordStrengthToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PasswordStrength ps = (PasswordStrength)value;

            switch(ps)
            {
                case PasswordStrength.VeryWeak:         return 0;
                case PasswordStrength.Weak:             return 25;
                case PasswordStrength.Medium:           return 50;
                case PasswordStrength.Strong:           return 75;
                case PasswordStrength.ExtremelyStrong:  return 100;
                default:                                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value == 0) return PasswordStrength.VeryWeak;
            if ((double)value == 25) return PasswordStrength.Weak;
            if ((double)value == 50) return PasswordStrength.Medium;
            if ((double)value == 75) return PasswordStrength.Strong;
            if ((double)value == 100) return PasswordStrength.ExtremelyStrong;

            return value;
        }
    }
}
