using Entity;
using Prism.Commands;
using Prism.Services.Dialogs;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Regions;
using UI.Infrastructure;
using UI.Model;
using UI.Views;

namespace UI.ViewModels;

internal class ScreenshotsViewModel : ViewModelBase<ScreenshotModel>
{
    private readonly DialogService dialogService;
    private DateOnly selectedDate;

    public ScreenshotsViewModel(DialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public ScreenshotModel? SelectedScreenshot { get; set; }

    private static ScreenshotService ScreenshotServiceInstance => ServiceLocator.GetService<ScreenshotService>();

    public DateOnly SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
            GetScreenshotsCommand.Execute(null);
        }
    }

    #region Command InitData - Команда Команда инициализировать данные на форме

    private ICommand? _InitDataCommand;

    /// <summary>Команда - Команда инициализировать данные на форме</summary>
    public ICommand InitDataCommand => _InitDataCommand
        ??= new DelegateCommand(OnInitDataCommandExecuted);

    private void OnInitDataCommandExecuted()
    {
        var today = DateTimeUtils.Today();
        // Так как при смене View коллекция Collection очищается,
        // а сеттер SelectedDate не будет вызван, если значение не изменилось,
        // то нужно принудительно вызвать команду получения скриншотов при неизменном значении SelectedDate 
        if(SelectedDate == today)
            GetScreenshotsCommand.Execute(null);
        SelectedDate = today;
    }

    #endregion

    #region Command GetScreenshots - Команда получить список скриншотов

    private ICommand? _GetScreenshotsCommand;

    /// <summary>Команда - получить список скриншотов</summary>
    public ICommand GetScreenshotsCommand => _GetScreenshotsCommand
        ??= new DelegateCommand(OnGetScreenshotsCommandExecuted);

    private async void OnGetScreenshotsCommandExecuted()
    {
        var screenshotsCount = await ScreenshotServiceInstance.GetScreensotsCountByDay(SelectedDate);

        // Заполнить список скриншотов предварительными данными, пока они не загрузились
        var screenshotData = new List<ScreenshotModel>(screenshotsCount);
        screenshotData
            .AddRange(Enumerable.Range(0, screenshotsCount)
            .Select(_ => new ScreenshotModel(null, DateTime.MinValue)));
        Collection = new ObservableCollection<ScreenshotModel>(screenshotData);

        // Параллельно загрузить список скриншотов
        const int bunchSize = 5;
        var bunchCount = screenshotsCount / bunchSize + 1;
        await Task.Run(() =>
            Parallel.For(0, bunchCount, async (bunchIndex, _) =>
                await FillScreenshotBunch(SelectedDate, bunchSize * bunchIndex, bunchSize, screenshotData)));
    }

    private static async Task FillScreenshotBunch(DateOnly date, int skip, int take, List<ScreenshotModel> screenshotData)
    {
        var screenshotBunch = await ScreenshotServiceInstance.GetScreenshotBunchByDay(date, skip, take);
        var screenshotBunchList = screenshotBunch.ToList();
        var screenshotBunchCount = screenshotBunchList.Count;
        var screenshotDataIndex = skip;
        for (var screenshotBunchIndex = 0;
             screenshotBunchIndex < screenshotBunchCount;
             screenshotBunchIndex++, screenshotDataIndex++)
        {
            var screenshotFromBunch = screenshotBunchList.ElementAt(screenshotBunchIndex);
            var screenshotModel = Map(screenshotFromBunch);
            // Нельзя создавать новый экземпляр элемента screenshotData,
            // поскольку на него ссылается Collection, которая отображается на View,
            // в противном случае потеряется связь между screenshotData и Collection
            screenshotData[screenshotDataIndex].Screenshot = screenshotModel.Screenshot;
            screenshotData[screenshotDataIndex].Created = screenshotModel.Created;
        }
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

    private static ScreenshotModel Map(Screenshot screenshot)
    {
        return new ScreenshotModel(screenshot.Data, screenshot.Created);
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        base.OnNavigatedFrom(navigationContext);
        // Изображения в WPF съедают невероятно много оперативки,
        // поэтому лучше очистить коллекцию и принудительно запустить сборщик мусора
        Collection.Clear();
        GC.Collect();
    }
}