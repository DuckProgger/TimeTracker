using Prism.Commands;
using Prism.Services.Dialogs;
using Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Infrastructure;
using UI.Model;
using UI.Views;
using MDDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace UI.ViewModels;

internal class WorkdayViewModel : ViewModelBase<WorkModel>
{
    private readonly IDialogService dialogService;
    private Timer? activeWorkRefresher;
    private DateOnly selectedDate;

    public WorkdayViewModel(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public string? NewWorkName { get; set; }

    public WorkdayModel? CurrentWorkday { get; set; }

    public DateOnly SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
            RefreshWorkCollection();
        }
    }

    public WorkModel? SelectedWork { get; set; }

    private static WorkdayService WorkdayServiceInstance => ServiceLocator.GetService<WorkdayService>();

    private async Task RefreshWorkCollection()
    {
        var workday = await WorkdayServiceInstance.GetByDate(SelectedDate);
        CurrentWorkday = workday != null ? WorkdayModel.Map(workday) : new WorkdayModel();
        var activeWork = CurrentWorkday?.Works.FirstOrDefault(w => w.IsActive);
        StopActiveWorkRefreshTimer();
        if (activeWork != null)
            StartActiveWorkRefreshTimer(activeWork);
    }

    private void StartActiveWorkRefreshTimer(WorkModel activeWork)
    {
        activeWorkRefresher ??= new Timer(_ =>
            activeWork.OnPropertyChanged(nameof(activeWork.TimeElapsed)), 
            null, 
            TimeSpan.Zero, 
            TimeSpan.FromSeconds(1));
    }

    private void StopActiveWorkRefreshTimer()
    {
        activeWorkRefresher?.Dispose();
        activeWorkRefresher = null;
    }

    #region Command InitData - Команда Команда инициализировать данные на форме

    private ICommand? _InitDataCommand;

    /// <summary>Команда - Команда инициализировать данные на форме</summary>
    public ICommand InitDataCommand => _InitDataCommand
        ??= new DelegateCommand(OnInitDataCommandExecuted);

    private void OnInitDataCommandExecuted()
    {
        SelectedDate = DateTimeUtils.Today();
    }

    #endregion

    #region Command AddWork - Команда добавить новую работу

    private ICommand? _AddWorkCommand;

    /// <summary>Команда - добавить новую работу</summary>
    public ICommand AddWorkCommand => _AddWorkCommand
        ??= new DelegateCommand(OnAddWorkCommandExecuted);

    private async void OnAddWorkCommandExecuted()
    {
        try
        {
            var createdWork = await WorkdayServiceInstance.AddWork(SelectedDate, NewWorkName);
            CurrentWorkday.Works.Add(WorkModel.Map(createdWork));
            NewWorkName = string.Empty;
        }
        catch (Exception e)
        {
            Notifier.AddError(e.Message);
        }
    }

    #endregion

    #region Command AddWorkloadManually - Команда добавить трудозатраты вручную

    private ICommand? _AddWorkloadManuallyCommand;

    /// <summary>Команда - добавить трудозатраты вручную</summary>
    public ICommand AddWorkloadManuallyCommand => _AddWorkloadManuallyCommand
        ??= new DelegateCommand(OnAddWorkloadManuallyCommandExecuted);

    private async void OnAddWorkloadManuallyCommandExecuted()
    {
        const string workloadParameterName = "workload";
        var dialogResult = await dialogService.ShowDialogAsync<AddWorkloadManuallyView>();
        if (dialogResult.Result != ButtonResult.OK) return;

        dialogResult.Parameters.TryGetValue<TimeSpan>(workloadParameterName, out var workload);
        await WorkdayServiceInstance.AddWorkload(SelectedWork.Id, workload);
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
            await WorkdayServiceInstance.StartRecording(SelectedDate, SelectedWork.Id);
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
            await WorkdayServiceInstance.StopRecording(SelectedDate, SelectedWork.Id);
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
        const string workParameterName = "work";
        var parameters = new DialogParameters { { workParameterName, SelectedWork } };
        var dialogResult = await dialogService.ShowDialogAsync<WorkView>(parameters);
        if (dialogResult.Result == ButtonResult.OK)
        {
            dialogResult.Parameters.TryGetValue<WorkModel>(workParameterName, out var work);
            await WorkdayServiceInstance.EditWork(work.Id, work.Name, work.Workload);
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
        await WorkdayServiceInstance.RemoveWork(SelectedWork.Id);
        MDDialogHost.CloseDialogCommand.Execute(null, null);
        await RefreshWorkCollection();
    }

    #endregion
}