using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Ghpr.Core.Utils
{
    internal static class Taker
    {
        public static string GetScreenName(DateTime now, ImageFormat format = null)
        {
            format = format ?? ImageFormat.Png;
            return $"img_{now.ToString("yyyyMMdd_HHmmssfff")}.{format.ToString().ToLower()}";
        }

        public static string TakeScreenshot(string screenPath, DateTime creationTime = default(DateTime))
        {
            if (!Directory.Exists(screenPath))
            {
                Directory.CreateDirectory(screenPath);
            }
            var format = ImageFormat.Png;
            var now = DateTime.Now;
            creationTime = creationTime.Equals(default(DateTime)) ? now : creationTime;
            var screenName = GetScreenName(creationTime, format);

            using (var bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height))
            {
                using (var g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);

                    var file = Path.Combine(screenPath, screenName);
                    bmpScreenCapture.Save(file, format);
                    var fileInfo = new FileInfo(file);
                    fileInfo.Refresh();
                    fileInfo.CreationTime = creationTime;

                }
            }
            return screenName;
        }

        public static string TakeScreenshot(string screenPath, Bitmap screen, DateTime creationTime = default(DateTime))
        {
            if (!Directory.Exists(screenPath))
            {
                Directory.CreateDirectory(screenPath);
            }
            var format = ImageFormat.Png;
            var now = DateTime.Now;
            creationTime = creationTime.Equals(default(DateTime)) ? now : creationTime;
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
