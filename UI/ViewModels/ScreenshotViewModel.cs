using System.Windows.Media;
using Prism.Services.Dialogs;

namespace UI.ViewModels;

internal class ScreenshotViewModel : DialogViewModelBase
{
    public ImageSource? DisplayedScreenshot { get; set; }

    public override void OnDialogOpened(IDialogParameters parameters)
    {
        if(parameters.TryGetValue<ImageSource>("screenshot", out var screenshot))
            DisplayedScreenshot = screenshot;
    }
}