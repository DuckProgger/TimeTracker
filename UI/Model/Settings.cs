using System;
using System.Collections.Generic;

namespace UI.Model;

internal class Settings : ModelBase
{
    public int ScreenshotsLifetimeFromDays { get; set; } = 7;

    public List<DayOfWeek> WorkDays { get; set; } = new List<DayOfWeek>()
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday
    };

    public TimeSpan WorkTimeBegin { get; set; } = TimeSpan.Parse("9:00");
    public TimeSpan WorkTimeEnd { get; set; } = TimeSpan.Parse("18:00");
}