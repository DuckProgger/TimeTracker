using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows.Input;
using UI.Model;

namespace UI.ViewModels;

internal class WorkViewModel : DialogViewModelBase
{
    public WorkModel? ProccessedWork { get; set; }

    #region Command Confirm - Команда подтвердить

    private ICommand? _ConfirmCommand;

    /// <summary>Команда - подтвердить</summary>
    public ICommand ConfirmCommand => _ConfirmCommand
        ??= new DelegateCommand(OnConfirmCommandExecuted);

    private void OnConfirmCommandExecuted()
    {
        RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters()
        {
            {"work", ProccessedWork}
        }));
    }

    #endregion

    public override void OnDialogOpened(IDialogParameters parameters)
    {
        parameters.TryGetValue<WorkModel>("work", out var work);
        ProccessedWork = work;
        Title = $"Редактирование работы {work.Name}";
    }
}