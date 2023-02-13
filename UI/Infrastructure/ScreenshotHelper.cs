using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media.Imaging;

namespace UI.Infrastructure;

internal class ScreenshotHelper
{
    public static byte[] CreateScreenshot(ImageFormat format)
    {
        var screenResolution = GetScreenResolution();
        var bitmap = new Bitmap(screenResolution.width, screenResolution.height);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
        return ConvertImageToBytes(bitmap, format);
    }

    public static BitmapImage CreateBitmapImage(byte[] imageBytes)
    {
        BitmapImage image = new();
        using (MemoryStream mem = new(imageBytes)) {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }
        image.Freeze();
        return image;
    }

    private static byte[] ConvertImageToBytes(Image image, ImageFormat format)
    {
        using var stream = new MemoryStream();
        image.Save(stream, format);
        return stream.ToArray();
    }

    private static Image ConvertBytesToImage(byte[] imageBytes)
    {
        using var stream = new MemoryStream(imageBytes);
        return new Bitmap(stream);
    }

    private static (int width, int height) GetScreenResolution()
    {
        return ((int)System.Windows.SystemParameters.PrimaryScreenWidth,
            (int)System.Windows.SystemParameters.PrimaryScreenHeight);
    }
}