﻿using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discreet_GUI.ValueConverters
{
    public class NegativeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double) return value;
            double v = (double)value;

            if (v <= 0) return value;

            return v * -1; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double) return value;
            double v = (double)value;

            if(v >= 0) return value;

            return Math.Abs(v);
        }
    }
}
