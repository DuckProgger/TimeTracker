using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Entity;
using Prism.Commands;
using Prism.Services.Dialogs;
using Services;
using UI.Infrastructure;
using UI.Model;
using UI.Views;

namespace UI.ViewModels;

internal class ScreenshotsViewModel : ViewModelBase<ScreenshotModel>
{
    private readonly ScreenshotService screenshotService;
    private readonly DialogService dialogService;

    public ScreenshotsViewModel(ScreenshotService screenshotService,
        DialogService dialogService)
    {
        this.screenshotService = screenshotService;
        this.dialogService = dialogService;
    }

    public ScreenshotModel? SelectedScreenshot { get; set; }

    #region Command GetScreenshots - Команда получить список скриншотов

    private ICommand? _GetScreenshotsCommand;

    /// <summary>Команда - получить список скриншотов</summary>
    public ICommand GetScreenshotsCommand => _GetScreenshotsCommand
        ??= new DelegateCommand(OnGetScreenshotsCommandExecuted);

    private async void OnGetScreenshotsCommandExecuted()
    {
        var screenshots = await screenshotService.GetByDay(DateTimeUtils.Today());
        Collection = new ObservableCollection<ScreenshotModel>(screenshots.Select(Map));
    }

    #endregion

    #region Command ShowScreenshot - Команда показать скриншот

    private ICommand? _ShowScreenshotCommand;

    /// <summary>Команда - показать скриншот</summary>
    public ICommand ShowScreenshotCommand => _ShowScreenshotCommand
        ??= new DelegateCommand(OnShowScreenshotCommandExecuted);

    private async void OnShowScreenshotCommandExecuted()
    {
        var parameters = new DialogParameters() { { "screenshot", SelectedScreenshot?.Screenshot } };
        await dialogService.ShowDialogAsync<ScreenshotView>(parameters);
    }

    #endregion

    private ScreenshotModel Map(Screenshot screenshot)
    {
        return new ScreenshotModel(screenshot.Data, screenshot.Created);
    }
}