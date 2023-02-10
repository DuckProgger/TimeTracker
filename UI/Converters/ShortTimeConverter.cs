using System;
using System.Globalization;
using System.Windows.Data;

namespace UI.Converters;

internal class ShortTimeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var time = (TimeSpan)value;
        return $"{time.Hours} ч {time.Minutes} мин";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
