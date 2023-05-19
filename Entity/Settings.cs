namespace Entity;

public class Settings
{
    public int ScreenshotsLifetimeFromDays { get; set; } = 7;

    public int ScreenshotCreationPeriodFromMinutes { get; set; } = 15;

    public List<DayOfWeek> WorkDays { get; set; } = new List<DayOfWeek>()
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday
    };

    /// <summary>
    /// Время начала рабочего дня
    /// </summary>
    public TimeSpan WorkTimeBegin { get; set; } = TimeSpan.Parse("9:00");

    /// <summary>
    /// Время окончания рабочего дня
    /// </summary>
    public TimeSpan WorkTimeEnd { get; set; } = TimeSpan.Parse("18:00");

    /// <summary>
    /// Продожительность обеда
    /// </summary>
    public TimeSpan LunchDuration { get; set; } = TimeSpan.Parse("1:00");
}