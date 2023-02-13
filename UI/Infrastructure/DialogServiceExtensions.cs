using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UI.Infrastructure;

internal static class DialogServiceExtensions
{
    public static Task<IDialogResult> ShowDialogAsync<TView>(this IDialogService dialogService, IDialogParameters? parameters = null)
    where TView : ContentControl
    {
        var tcs = new TaskCompletionSource<IDialogResult>();
        dialogService.ShowDialog(typeof(TView).Name, parameters, result => tcs.SetResult(result));
        return tcs.Task;
    }
}