using System;
using System.Globalization;
using System.Windows.Data;
using UI.Infrastructure;

namespace UI.Converters;

internal class DateTimeToTimeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateTimeString = value.ToString();
        if (!DateTime.TryParse(dateTimeString, out var dateTime))
            return string.Empty;
        var time = dateTime.TimeOfDay;
        return DateTimeUtils.ToShortTimeString(time);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeString = (string)value;
        return TimeSpan.Parse(timeString);
    }
}
