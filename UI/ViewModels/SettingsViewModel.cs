using Prism.Commands;
using System;
using System.Windows.Input;
using UI.Model;
using UI.Services;

namespace UI.ViewModels;

internal class SettingsViewModel : ViewModelBase
{
    public Settings Settings { get; set; }

    public bool IsMondaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Monday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Monday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Monday);
        }
    }

    public bool IsTuesdaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Tuesday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Tuesday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Tuesday);
        }
    }

    public bool IsWednesdaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Wednesday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Wednesday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Wednesday);
        }
    }

    public bool IsThursdaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Thursday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Thursday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Thursday);
        }
    }

    public bool IsFridaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Friday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Friday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Friday);
        }
    }

    public bool IsSaturdaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Saturday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Saturday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Saturday);
        }
    }

    public bool IsSundaySelected
    {
        get => Settings?.WorkDays?.Contains(DayOfWeek.Sunday) ?? false;
        set
        {
            if (value)
                Settings.WorkDays.Add(DayOfWeek.Sunday);
            else
                Settings.WorkDays.Remove(DayOfWeek.Sunday);
        }
    }

    #region Command InitData - Команда Команда инициализировать данные на форме

    private ICommand? _InitDataCommand;

    /// <summary>Команда - Команда инициализировать данные на форме</summary>
    public ICommand InitDataCommand => _InitDataCommand
        ??= new DelegateCommand(OnInitDataCommandExecuted);

    private void OnInitDataCommandExecuted()
    {
        Settings = SettingsService.Read();
    }

    #endregion

    #region Command Save - Команда сохранить настройки

    private ICommand? _SaveCommand;

    /// <summary>Команда - сохранить настройки</summary>
    public ICommand SaveCommand => _SaveCommand
        ??= new DelegateCommand(OnSaveCommandExecuted);

    private void OnSaveCommandExecuted()
    {
        SettingsService.Save(Settings);
    }

    #endregion

    #region Command Cancel - Команда отменить

    private ICommand? _CancelCommand;

    /// <summary>Команда - отменить</summary>
    public ICommand CancelCommand => _CancelCommand
        ??= new DelegateCommand(OnCancelCommandExecuted);

    private void OnCancelCommandExecuted()
    {
        InitDataCommand.Execute(null);
    }

    #endregion
}