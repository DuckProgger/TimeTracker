using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Entity;
using Prism.Commands;
using Services;
using UI.Infrastructure;
using UI.Model;

namespace UI.ViewModels;

internal class ScreenshotsViewModel : ViewModelBase<ScreenshotModel>
{
    private readonly ScreenshotService screenshotService;

    public ScreenshotsViewModel(ScreenshotService screenshotService)
    {
        this.screenshotService = screenshotService;
    }

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

    private ScreenshotModel Map(Screenshot screenshot)
    {
        return new ScreenshotModel(screenshot.Data, screenshot.Created);
    }
}