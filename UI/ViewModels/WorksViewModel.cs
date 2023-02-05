﻿using System.Windows.Input;
using Prism.Commands;
using Services;
using UI.Model;

namespace UI.ViewModels;

internal class WorksViewModel : ViewModelBase<WorkModel>
{
    private readonly WorkService workService;

    public WorksViewModel(WorkService workService)
    {
        this.workService = workService;
    }

    public WorkModel NewWork { get; set; } = new();

    #region Command AddWork - Команда добавить новую работу

    private ICommand? _AddWorkCommand;

    /// <summary>Команда - добавить новую работу</summary>
    public ICommand AddWorkCommand => _AddWorkCommand
        ??= new DelegateCommand(OnAddWorkCommandExecuted);

    private async void OnAddWorkCommandExecuted()
    {
        await workService.AddWork(NewWork.Name);
    }

    #endregion
}