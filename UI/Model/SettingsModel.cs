using System;
using System.Collections.Generic;
using Entity;

namespace UI.Model;

internal class SettingsModel : ModelBase
{
    public int ScreenshotsLifetimeFromDays { get; set; }

    public int ScreenshotCreationPeriodFromMinutes { get; set; }

    public List<DayOfWeek> WorkDays { get; set; }

    /// <summary>
    /// Время начала рабочего дня
    /// </summary>
    public TimeSpan WorkTimeBegin { get; set; }

    /// <summary>
    /// Время окончания рабочего дня
    /// </summary>
    public TimeSpan WorkTimeEnd { get; set; }

    /// <summary>
    /// Продожительность обеда
    /// </summary>
    public TimeSpan LunchDuration { get; set; }

    public static SettingsModel Map(Settings settings)
    {
        return new SettingsModel()
        {
            WorkTimeBegin = settings.WorkTimeBegin,
            WorkTimeEnd = settings.WorkTimeEnd,
            LunchDuration = settings.LunchDuration,
            WorkDays = settings.WorkDays,
            ScreenshotCreationPeriodFromMinutes = settings.ScreenshotCreationPeriodFromMinutes,
            ScreenshotsLifetimeFromDays = settings.ScreenshotsLifetimeFromDays,
        };
    }

    public static Settings MapReverse(SettingsModel settingsModel)
    {
        return new Settings()
        {
            WorkTimeBegin = settingsModel.WorkTimeBegin,
            WorkTimeEnd = settingsModel.WorkTimeEnd,
            LunchDuration = settingsModel.LunchDuration,
            WorkDays = settingsModel.WorkDays,
            ScreenshotCreationPeriodFromMinutes = settingsModel.ScreenshotCreationPeriodFromMinutes,
            ScreenshotsLifetimeFromDays = settingsModel.ScreenshotsLifetimeFromDays,
        };
    }
}