using System;
using System.Globalization;
using System.Windows.Data;
using UI.Infrastructure;

namespace UI.Converters;

internal class FractionTimeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var time = (TimeSpan)value;
        var hoursPart = time.Hours;
        // Преобразовать минуты в сотые доли часа (45 мин -> 75 (0.75 часа))
        var minutesPart = DateTimeUtils.ToStringWithZero((int)double.Round(time.Minutes * 1.66));
        return $"{hoursPart}.{minutesPart} ч";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
