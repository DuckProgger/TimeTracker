using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows.Input;
using UI.Infrastructure;
using UI.Validation.Rules;

namespace UI.ViewModels;

internal class AddWorkloadManuallyViewModel : DialogViewModelBase
{
    public AddWorkloadManuallyViewModel()
    {
        Title = "Добавить трудозатраты";
        AddValidator(() => ProccessedWorkload, new IncorrectTimeSpanFormatValidationRule());
    }

    public string ProccessedWorkload { get; set; } = "00:00";

    #region Command Confirm - Команда Ок

    private ICommand? _ConfirmCommand;

    /// <summary>Команда - Ок</summary>
    public ICommand ConfirmCommand => _ConfirmCommand
        ??= new DelegateCommand(OnConfirmCommandExecuted).ObservesCanExecute(() => ValidationSuccess);

    private void OnConfirmCommandExecuted()
    {
        DateTimeUtils.TryParse(ProccessedWorkload, out var workload);
        RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters() { { "workload", workload } }));
    }

    #endregion
    
    #region Command Close - Команда Закрыть окно

    private ICommand? _CloseCommand;

    /// <summary>Команда - Закрыть окно</summary>
    public ICommand CloseCommand => _CloseCommand
        ??= new DelegateCommand(OnCloseCommandExecuted);

    private void OnCloseCommandExecuted()
    {
        RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
    }

    #endregion
}