using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Utility;

namespace WPF.ValueConverters
{
    public class PasswordStrengthToColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PasswordStrength ps = (PasswordStrength)value;

            switch(ps)
            {
                case PasswordStrength.VeryWeak:         return Brush.Parse("#EB1435");
                case PasswordStrength.Weak:             return Brush.Parse("#EB1435");
                case PasswordStrength.Medium:           return Brush.Parse("#F3BC0C");
                case PasswordStrength.Strong:           return Brush.Parse("#45D728");
                case PasswordStrength.ExtremelyStrong:  return Brush.Parse("#45D728");
                default:                                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
