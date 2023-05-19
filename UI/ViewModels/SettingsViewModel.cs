using Prism.Commands;
using System;
using System.Windows.Input;
using Services;
using UI.Model;
using UI.Services;

namespace UI.ViewModels;

internal class SettingsViewModel : ViewModelBase
{
    public SettingsModel SettingsModel { get; set; }

    public bool IsMondaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Monday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Monday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Monday);
        }
    }

    public bool IsTuesdaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Tuesday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Tuesday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Tuesday);
        }
    }

    public bool IsWednesdaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Wednesday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Wednesday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Wednesday);
        }
    }

    public bool IsThursdaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Thursday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Thursday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Thursday);
        }
    }

    public bool IsFridaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Friday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Friday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Friday);
        }
    }

    public bool IsSaturdaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Saturday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Saturday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Saturday);
        }
    }

    public bool IsSundaySelected
    {
        get => SettingsModel?.WorkDays?.Contains(DayOfWeek.Sunday) ?? false;
        set
        {
            if (value)
                SettingsModel.WorkDays.Add(DayOfWeek.Sunday);
            else
                SettingsModel.WorkDays.Remove(DayOfWeek.Sunday);
        }
    }

    #region Command InitData - Команда Команда инициализировать данные на форме

    private ICommand? _InitDataCommand;

    /// <summary>Команда - Команда инициализировать данные на форме</summary>
    public ICommand InitDataCommand => _InitDataCommand
        ??= new DelegateCommand(OnInitDataCommandExecuted);

    private void OnInitDataCommandExecuted()
    {
        var settings = SettingsService.Read();
        SettingsModel = SettingsModel.Map(settings);
    }

    #endregion

    #region Command Save - Команда сохранить настройки

    private ICommand? _SaveCommand;

    /// <summary>Команда - сохранить настройки</summary>
    public ICommand SaveCommand => _SaveCommand
        ??= new DelegateCommand(OnSaveCommandExecuted);

    private void OnSaveCommandExecuted()
    {
        var settings = SettingsModel.MapReverse(SettingsModel);
        SettingsService.Save(settings);
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