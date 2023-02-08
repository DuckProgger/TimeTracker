using System;
using Entity;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using UI.Infrastructure;
using UI.Model;

namespace UI.ViewModels;

internal class WorkViewModel : ViewModelBase
{
    public WorkModel ProccessedWork { get; set; }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters["work"] is not WorkModel work)
            throw new Exception($"parameter {nameof(navigationContext)} must be of {nameof(WorkModel)} type.");
        ProccessedWork = work;
    }

    #region Command Confirm - Команда подтвердить

    private ICommand? _ConfirmCommand;

    /// <summary>Команда - подтвердить</summary>
    public ICommand ConfirmCommand => _ConfirmCommand
        ??= new DelegateCommand(OnConfirmCommandExecuted);

    private void OnConfirmCommandExecuted()
    {
        DialogService dialogService = ServiceLocator.GetService<DialogService>();
        dialogService.AddParameter("work2", ProccessedWork);
        dialogService.SetResult(DialogService.DialogResults.Ok);
        dialogService.GoBack();
    }

    #endregion
}