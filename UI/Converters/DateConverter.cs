using System;
using System.Globalization;
using System.Windows.Data;

namespace UI.Converters;

internal class DateConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateOnly = (DateOnly)value;
        return dateOnly.ToDateTime(TimeOnly.MinValue);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = (DateTime)value;
        return DateOnly.FromDateTime(date);
    }
}
