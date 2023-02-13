using System;
using System.Windows.Media;
using UI.Infrastructure;

namespace UI.Model;

internal class ScreenshotModel
{
    public ScreenshotModel(byte[] imageBytes, DateTime created)
    {
        Screenshot = ScreenshotHelper.CreateBitmapImage(imageBytes);
        Created = created;
    }

    public ImageSource Screenshot { get; }

    public DateTime Created { get; set; }
}