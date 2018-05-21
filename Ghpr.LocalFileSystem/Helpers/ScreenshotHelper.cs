using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.Core.Utils;

namespace Ghpr.LocalFileSystem.Helpers
{
    public static class ScreenshotHelper
    {
        public static string GetScreenKey(int count)
        {
            return $"{Paths.Names.ScreenshotKeyTemplate}{count}";
        }

        public static string GetScreenName(DateTime dt)
        {
            var format = ImageFormat.Png;
            return $"img_{dt:yyyyMMdd_HHmmssfff}.{format.ToString().ToLower()}";
        }

        public static string SaveScreenshot(string screenPath, byte[] screenBytes, DateTime creationTime)
        {
            using (var image = Image.FromStream(new MemoryStream(screenBytes)))
            {
                var format = ImageFormat.Png;
                screenPath.Create();
                creationTime = creationTime.Equals(default(DateTime)) ? DateTime.Now : creationTime;
                var screenName = GetScreenName(creationTime);
                var file = Path.Combine(screenPath, screenName);
                var screen = new Bitmap(image);
                screen.Save(file, format);

                var fileInfo = new FileInfo(file);
                fileInfo.Refresh();
                fileInfo.CreationTime = creationTime;

                return screenName;
            }
        }
    }
}
