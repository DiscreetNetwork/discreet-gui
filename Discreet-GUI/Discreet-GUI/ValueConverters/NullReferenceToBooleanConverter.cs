using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class NullReferenceToBooleanConverter : IValueConverter
    {
        enum Parameter
        {
            IsNull,
            NotNull
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);

            return param == Parameter.IsNull ? value == null : value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
