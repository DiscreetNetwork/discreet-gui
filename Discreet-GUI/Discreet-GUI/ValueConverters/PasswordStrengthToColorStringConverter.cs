using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using Discreet_GUI.Utility;

namespace Discreet_GUI.ValueConverters
{
    public class PasswordStrengthToColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PasswordStrength ps = (PasswordStrength)value;

            switch(ps)
            {
                case PasswordStrength.VeryWeak:         return Brush.Parse("#a20e24");
                case PasswordStrength.Weak:             return Brush.Parse("#a20e24");
                case PasswordStrength.Medium:           return Brush.Parse("#c79a0a");
                case PasswordStrength.Strong:           return Brush.Parse("#2a8318");
                case PasswordStrength.ExtremelyStrong:  return Brush.Parse("#2a8318");
                default:                                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
