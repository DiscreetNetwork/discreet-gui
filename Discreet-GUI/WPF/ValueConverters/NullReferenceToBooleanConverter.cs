using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ValueConverters
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
