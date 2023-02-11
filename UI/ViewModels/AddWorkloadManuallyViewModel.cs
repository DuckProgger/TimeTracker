using Prism.Commands;
using System;
using System.Windows.Input;
using Prism.Services.Dialogs;

namespace UI.ViewModels;

internal class AddWorkloadManuallyViewModel : DialogViewModelBase
{
    public AddWorkloadManuallyViewModel()
    {
        Title = "Добавить трудозатраты";
    }

    public TimeSpan ProccessedWorkload { get; set; }

    #region Command Confirm - Команда Ок

    private ICommand? _ConfirmCommand;

    /// <summary>Команда - Ок</summary>
    public ICommand ConfirmCommand => _ConfirmCommand
        ??= new DelegateCommand(OnConfirmCommandExecuted);

    private void OnConfirmCommandExecuted()
    {
        RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters() { { "workload", ProccessedWorkload } }));
    }

    #endregion
}