using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using UI.Infrastructure;
using UI.Model;

namespace UI.ViewModels;

internal class SettingsViewModel : ViewModelBase
{
    public Settings Settings { get; set; }

    #region Command InitData - Команда Команда инициализировать данные на форме

    private ICommand? _InitDataCommand;

    /// <summary>Команда - Команда инициализировать данные на форме</summary>
    public ICommand InitDataCommand => _InitDataCommand
        ??= new DelegateCommand(OnInitDataCommandExecuted);

    private async void OnInitDataCommandExecuted()
    {
        Settings = await SettingsService.Read();
    }

    #endregion

    #region Command Save - Команда сохранить настройки

    private ICommand? _SaveCommand;

    /// <summary>Команда - сохранить настройки</summary>
    public ICommand SaveCommand => _SaveCommand
        ??= new DelegateCommand(OnSaveCommandExecuted);

    private async void OnSaveCommandExecuted()
    {
        await SettingsService.Save(Settings);
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