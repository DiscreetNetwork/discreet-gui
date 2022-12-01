using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Discreet_GUI.Utility;

namespace Discreet_GUI.ValueConverters
{
    public class PasswordStrengthToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PasswordStrength ps = (PasswordStrength)value;

            switch(ps)
            {
                case PasswordStrength.VeryWeak:         return "Very weak";
                case PasswordStrength.Weak:             return "Weak";
                case PasswordStrength.Medium:           return "Medium";
                case PasswordStrength.Strong:           return "Strong";
                case PasswordStrength.ExtremelyStrong:  return "Extremely strong";
                default:                                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
