using System.Drawing;
using System.Drawing.Imaging;

namespace UI.Services;

public class PrintScreenService
{
    private readonly int screenWidth;
    private readonly int screenHeigth;
    private readonly string path;

    public PrintScreenService(int screenWidth, int screenHeigth, string path)
    {
        this.screenWidth = screenWidth;
        this.screenHeigth = screenHeigth;
        this.path = path;
    }

    public void PrintScreen(string fileName)
    {
        Bitmap bitmap = new Bitmap(screenWidth, screenHeigth);
        Graphics graphics = Graphics.FromImage(bitmap as Image);
        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
        bitmap.Save($@"{path}\{fileName}.jpg", ImageFormat.Jpeg);
    }
}