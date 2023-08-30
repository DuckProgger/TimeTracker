using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows.Input;
using UI.Infrastructure;
using UI.Model;
using UI.Validation.Rules;

namespace UI.ViewModels;

internal class WorkViewModel : DialogViewModelBase
{
    public WorkViewModel()
    {
        AddValidator(() => ProccessedWorkload, new IncorrectTimeSpanFormatValidationRule());
    }

    public WorkModel ProccessedWork { get; set; } = null!;

    public string? ProccessedWorkload { get; set; }

    #region Command Confirm - Команда подтвердить

    private ICommand? _ConfirmCommand;

    /// <summary>Команда - подтвердить</summary>
    public ICommand ConfirmCommand => _ConfirmCommand
        ??= new DelegateCommand(OnConfirmCommandExecuted).ObservesCanExecute(() => ValidationSuccess);

    private void OnConfirmCommandExecuted()
    {
        DateTimeUtils.TryParse(ProccessedWorkload, out var workload);
        ProccessedWork.Workload = workload;
        
        RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters()
        {
            {"work", ProccessedWork}
        }));
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

    public override void OnDialogOpened(IDialogParameters parameters)
    {
        parameters.TryGetValue<WorkModel>("work", out var work);
        ProccessedWork = work;
        ProccessedWorkload = DateTimeUtils.ToShortTimeString(work.Workload);
        Title = $"Редактирование работы {work.Name}";
    }
}