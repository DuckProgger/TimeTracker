using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using UI.Infrastructure;

namespace UI.Converters;

internal class TimeConverter : IValueConverter
{
    private readonly Regex timeRegex = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}", RegexOptions.Compiled);
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeString = value.ToString();
        var timeMatch = timeRegex.Match(timeString);
        var time = TimeSpan.Parse(timeMatch.Value);
        return $"{DateTimeUtils.ToStringWithZero(time.Hours)}:{DateTimeUtils.ToStringWithZero(time.Minutes)}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeString = (string)value;
        return TimeSpan.Parse(timeString);
    }
}
