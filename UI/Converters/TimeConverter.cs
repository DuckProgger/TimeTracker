﻿using System;
using System.Globalization;
using System.Windows.Data;
using UI.Infrastructure;

namespace UI.Converters;

internal class TimeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var time = (TimeSpan)value;
        return $"{DateTimeUtils.ToStringWithZero(time.Hours)}:{DateTimeUtils.ToStringWithZero(time.Minutes)}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeString = (string)value;
        return TimeSpan.Parse(timeString);
    }
}
