using System;
using Prism.Commands;
using System.Windows.Input;
using UI.Infrastructure;

namespace UI.ViewModels;

internal class AddWorkloadManuallyViewModel : ViewModelBase
{
    public AddWorkloadManuallyViewModel()
    {
        Title = "Добавить трудозатраты";
    }

    public TimeSpan SelectedWorkload { get; set; }

    #region Command Confirm - Команда Ок

    private ICommand? _ConfirmCommand;

    /// <summary>Команда - Ок</summary>
    public ICommand ConfirmCommand => _ConfirmCommand
        ??= new DelegateCommand(OnConfirmCommandExecuted);

    private async void OnConfirmCommandExecuted()
    {
        DialogService dialogService = ServiceLocator.GetService<DialogService>();
        dialogService.AddParameter("workload", SelectedWorkload);
        dialogService.SetResult(DialogService.DialogResults.Ok);
        dialogService.GoBack();
    }

    #endregion
}