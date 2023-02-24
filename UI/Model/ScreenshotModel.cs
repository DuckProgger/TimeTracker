using System;
using System.Windows.Media;
using UI.Infrastructure;

namespace UI.Model;

internal class ScreenshotModel : ModelBase
{
    public ScreenshotModel(byte[]? imageBytes, DateTime created)
    {
        if (imageBytes != null)
            Screenshot = ScreenshotHelper.CreateBitmapImage(imageBytes);
        Created = created;
    }

    public ImageSource? Screenshot { get; internal set; }

    public DateTime Created { get; internal set; }
}