using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Services;
using UI.Infrastructure;
using UI.Model;
using UI.Views;

namespace UI.ViewModels;

internal class WorksViewModel : ViewModelBase<WorkModel>
{
    private readonly WorkdayService workdayService;

    public WorksViewModel(WorkdayService workdayService)
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
}