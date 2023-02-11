using Prism.Services.Dialogs;
using System;

namespace UI.ViewModels;

internal abstract class DialogViewModelBase : ViewModelBase, IDialogAware
{
    public virtual bool CanCloseDialog() => true;

    public virtual void OnDialogClosed() { }

    public virtual void OnDialogOpened(IDialogParameters parameters) { }

    public event Action<IDialogResult>? RequestClose;

    public virtual void RaiseRequestClose(IDialogResult dialogResult)
    {
        RequestClose?.Invoke(dialogResult);
    }
}