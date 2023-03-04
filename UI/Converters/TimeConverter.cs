using System;
using System.Globalization;
using System.Windows.Data;
using UI.Infrastructure;

namespace UI.Converters;

internal class TimeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeString = value.ToString();
        var time = TimeSpan.Parse(timeString);
        return DateTimeUtils.ToShortTimeString(time);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeString = (string)value;
        return TimeSpan.Parse(timeString);
    }
}
