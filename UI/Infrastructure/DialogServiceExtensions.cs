using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UI.Infrastructure;

internal static class DialogServiceExtensions
{
    public static Task<IDialogResult> ShowDialogAsync<T>(this IDialogService dialogService, IDialogParameters? parameters = null)
    where T : ContentControl
    {
        var tcs = new TaskCompletionSource<IDialogResult>();
        dialogService.ShowDialog(typeof(T).Name, parameters, result => tcs.SetResult(result));
        return tcs.Task;
    }
}