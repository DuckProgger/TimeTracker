using System;

namespace UI.Infrastructure;

internal class DateTimeUtils
{
    public static DateOnly Today()
    {
        var today = DateTime.Today.Date;
        return new DateOnly(today.Year, today.Month, today.Day);
    }
    public static string ToStringWithZero(int value)
    {
        return value < 10
            ? $"0{value}"
            : value.ToString();
    }
}