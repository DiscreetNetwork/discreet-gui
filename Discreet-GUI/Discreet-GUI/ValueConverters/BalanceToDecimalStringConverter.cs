﻿using Avalonia.Data.Converters;
using Services;
using System;
using System.Globalization;

namespace Discreet_GUI.ValueConverters
{
    public class BalanceToDecimalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ulong val = (ulong)value;

            decimal? balanceDivided = DISTConverter.Divide(val);

            return balanceDivided is null ? "NaN" : DISTConverter.ToStringFormat(balanceDivided.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
