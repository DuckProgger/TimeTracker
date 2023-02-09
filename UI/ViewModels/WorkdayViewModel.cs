using Prism.Commands;
using Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Infrastructure;
using UI.Model;
using UI.Views;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace UI.ViewModels;

internal class WorkdayViewModel : ViewModelBase<WorkModel>
{
    private readonly WorkdayService workdayService;

    public WorkdayViewModel(WorkdayService workdayService)
    {
        this.workdayService = workdayService;
    }

    public WorkModel NewWork { get; set; } = new();

    public DateOnly SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
            RefreshWorkCollection();
        }
    }

    public WorkModel SelectedWork { get; set; }

    private async Task RefreshWorkCollection()
    {
        var works = await workdayService.GetWorks(SelectedDate);
        Collection = new ObservableCollection<WorkModel>(works.Select(WorkModel.Map));
    }

    #region Command InitData - Команда Команда инициализировать данные на форме

    private ICommand? _InitDataCommand;

    /// <summary>Команда - Команда инициализировать данные на форме</summary>
    public ICommand InitDataCommand => _InitDataCommand
        ??= new DelegateCommand(OnInitDataCommandExecuted);

    private void OnInitDataCommandExecuted()
    {
        SelectedDate = DateOnlyHelper.Today();
    }

    #endregion

    #region Command AddWork - Команда добавить новую работу

    private ICommand? _AddWorkCommand;
    private DateOnly selectedDate;

    /// <summary>Команда - добавить новую работу</summary>
    public ICommand AddWorkCommand => _AddWorkCommand
        ??= new DelegateCommand(OnAddWorkCommandExecuted);

    private async void OnAddWorkCommandExecuted()
    {
        if (string.IsNullOrWhiteSpace(NewWork.Name))
        {
            Notifier.AddError("Название работы не может быть пустым.");
            return;
        }
        var createdWork = await workdayService.AddWork(SelectedDate, NewWork.Name);
        Collection.Add(WorkModel.Map(createdWork));
        NewWork = new();
    }

    #endregion

    #region Command AddWorkloadManually - Команда добавить трудозатраты вручную

    private ICommand? _AddWorkloadManuallyCommand;

    /// <summary>Команда - добавить трудозатраты вручную</summary>
    public ICommand AddWorkloadManuallyCommand => _AddWorkloadManuallyCommand
        ??= new DelegateCommand(OnAddWorkloadManuallyCommandExecuted);

    private async void OnAddWorkloadManuallyCommandExecuted()
    {
        DialogService dialogService = ServiceLocator.GetService<DialogService>();
        var result = await dialogService.ShowDialog(nameof(AddWorkloadManuallyView));
        if (result.Parameters.TryGetValue<TimeSpan>("workload", out var workload))
        {
            await workdayService.AddWorkload(SelectedWork.Id, workload);
        }

        await RefreshWorkCollection();
    }

    #endregion

    #region Command StartRecording - Команда запустить таймер для работы

    private ICommand? _StartRecordingCommand;

    /// <summary>Команда - запустить таймер для работы</summary>
    public ICommand StartRecordingCommand => _StartRecordingCommand
        ??= new DelegateCommand(OnStartRecordingCommandExecuted);

    private async void OnStartRecordingCommandExecuted()
    {
        try
        {
            await workdayService.StartRecording(SelectedWork.Id);
            await RefreshWorkCollection();
        }
        catch (Exception e)
        {
            Notifier.AddError(e.Message);
        }
    }

    #endregion

    #region Command StopRecording - Команда остановить таймер для работы

    private ICommand? _StopRecordingCommand;

    /// <summary>Команда - остановить таймер для работы</summary>
    public ICommand StopRecordingCommand => _StopRecordingCommand
        ??= new DelegateCommand(OnStopRecordingCommandExecuted);

    private async void OnStopRecordingCommandExecuted()
    {
        try
        {
            await workdayService.StopRecording(SelectedWork.Id);
            await RefreshWorkCollection();
        }
        catch (Exception e)
        {
            Notifier.AddError(e.Message);
        }
    }

    #endregion

    #region Command EditWork - Команда редактировать работу

    private ICommand? _EditWorkCommand;

    /// <summary>Команда - редактировать работу</summary>
    public ICommand EditWorkCommand => _EditWorkCommand
        ??= new DelegateCommand(OnEditWorkCommandExecuted);

    private async void OnEditWorkCommandExecuted()
    {
        // TODO сделать механизм типа AddOrChangeParameter, вынести строки в константы
        DialogService dialogService = ServiceLocator.GetService<DialogService>();
        dialogService.AddParameter("work", SelectedWork);
        var result = await dialogService.ShowDialog(nameof(WorkView));
        if (result.Parameters.TryGetValue<WorkModel>("work2", out var work))
        {
            await workdayService.EditWork(work.Id, work.Name, work.Workload);
        }

        await RefreshWorkCollection();
    }

    #endregion

    #region Command DeleteWork - Команда удалить работу

    private ICommand? _DeleteWorkCommand;

    /// <summary>Команда - удалить работу</summary>
    public ICommand DeleteWorkCommand => _DeleteWorkCommand
        ??= new DelegateCommand(OnDeleteWorkCommandExecuted);

    private async void OnDeleteWorkCommandExecuted()
    {
        await workdayService.RemoveWork(SelectedWork.Id);
        MDDialogHost.CloseDialogCommand.Execute(null, null);
        await RefreshWorkCollection();
    }

    #endregion
}