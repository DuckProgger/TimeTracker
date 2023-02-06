using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Services;
using UI.Infrastructure;
using UI.Model;

namespace UI.ViewModels;

internal class WorksViewModel : ViewModelBase<WorkModel>
{
    private readonly WorkdayService workdayService;

    public WorksViewModel(WorkdayService workdayService)
    {
        this.workdayService = workdayService;
    }

    public WorkModel NewWork { get; set; } = new();
    public DateOnly SelectedDate { get; set; }
    public WorkModel SelectedWork { get; set; }

    #region Command InitDataCommand - Команда инициализировать данные на форме

    private ICommand? _InitDataCommandCommand;

    /// <summary>Команда - инициализировать данные на форме</summary>
    public ICommand InitDataCommandCommand => _InitDataCommandCommand
        ??= new DelegateCommand(OnInitDataCommandCommandExecuted);

    private async void OnInitDataCommandCommandExecuted()
    {
        SelectedDate = DateOnlyHelper.Today();
        var works = await workdayService.GetWorks(SelectedDate);
        Collection = new ObservableCollection<WorkModel>(works.Select(WorkModel.Map));
    }

    #endregion

    #region Command AddWork - Команда добавить новую работу

    private ICommand? _AddWorkCommand;

    /// <summary>Команда - добавить новую работу</summary>
    public ICommand AddWorkCommand => _AddWorkCommand
        ??= new DelegateCommand(OnAddWorkCommandExecuted);

    private async void OnAddWorkCommandExecuted()
    {
        var createdWork = await workdayService.AddWork(SelectedDate, NewWork.Name);
        Collection.Add(WorkModel.Map(createdWork));
    }

    #endregion
}