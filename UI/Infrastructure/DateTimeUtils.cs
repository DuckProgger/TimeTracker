using System;
using System.Text.RegularExpressions;

namespace UI.Infrastructure;

internal class DateTimeUtils
{
    private static readonly Regex timeRegex =
        new(@"^(?:(?'hours'[01]?\d|2[0-3]):(?'minutes'[0-5]?\d)|(?:(?'hours'[2][0-3])(?'minutes'[0-5]\d)|(?'hours'[01]?\d?)(?'minutes'[0-5]\d)|(?'minutes'\d)))$", RegexOptions.Compiled);

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

    public static string ToShortTimeString(TimeSpan time) =>
     $"{ToStringWithZero(time.Hours)}:{ToStringWithZero(time.Minutes)}";

    /// <summary>
    /// Попытаться преобразовать строку, содержащую время в <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="timeString">Строка, содержащая время.</param>
    /// <param name="timeSpan">Временной интервал.</param>
    /// <returns>true, если преобразование успешно; иначе false.</returns>
    public static bool TryParse(string timeString, out TimeSpan timeSpan)
    {
        if (!CheckCorrectTimeFormat(timeString))
        {
            timeSpan = TimeSpan.Zero;
            return false;
        }
        var timeMatch = timeRegex.Match(timeString);
        var hoursMatch = timeMatch.Groups["hours"].Value;
        string hours = string.IsNullOrEmpty(hoursMatch)
            ? "0"
            : hoursMatch;
        var minutes = timeMatch.Groups["minutes"].Value;
        return TimeSpan.TryParse($"{hours}:{minutes}", out timeSpan);
    }

    /// <summary>
    /// Проверить правильность формата строки, содержащей время.
    /// </summary>
    /// <param name="timeString">Строка, содержащая время.</param>
    /// <returns>true, если строка корректная; иначе false.</returns>
    public static bool CheckCorrectTimeFormat(string timeString)
    {
        return timeRegex.IsMatch(timeString);
    }
}