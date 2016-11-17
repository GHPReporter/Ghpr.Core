using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Ghpr.Core.Utils
{
    internal static class Taker
    {
        public static void TakeScreenshot(string screenPath, ImageFormat format, DateTime creationTime = default(DateTime))
        {
            var b = Screen.PrimaryScreen.Bounds;
            using (var btm = new Bitmap(b.Width, b.Height))
            {
                using (var g = Graphics.FromImage(btm))
                {
                    g.CopyFromScreen(b.X, b.Y, 0, 0, btm.Size, CopyPixelOperation.SourceCopy);

                    SaveScreenshot(screenPath, btm, format, creationTime);
                }
            }
        }

        public static string GetScreenName(DateTime now, ImageFormat format)
        {
            format = format ?? ImageFormat.Png;
            return $"img_{now.ToString("yyyyMMdd_HHmmssfff")}.{format.ToString().ToLower()}";
        }

        public static string SaveScreenshot(string screenPath, byte[] screenBytes,
            ImageFormat format, DateTime creationTime = default(DateTime))
        {
            using (var image = Image.FromStream(new MemoryStream(screenBytes)))
            {
                return SaveScreenshot(screenPath, new Bitmap(image), format, creationTime);
            }
        }

        public static string SaveScreenshot(string screenPath, Image img,
            ImageFormat format, DateTime creationTime = default(DateTime))
        {
            return SaveScreenshot(screenPath, new Bitmap(img), format, creationTime);
        }

        public static string SaveScreenshot(string screenPath, Bitmap screen,
            ImageFormat format, DateTime creationTime = default(DateTime))
        {
            if (!Directory.Exists(screenPath))
            {
                Directory.CreateDirectory(screenPath);
            }
            creationTime = creationTime.Equals(default(DateTime)) ? DateTime.Now : creationTime;
            var screenName = GetScreenName(creationTime, format);
            var file = Path.Combine(screenPath, screenName);

            screen.Save(file, format);

            var fileInfo = new FileInfo(file);
            fileInfo.Refresh();
            fileInfo.CreationTime = creationTime;

            return screenName;
        }
    }
}
