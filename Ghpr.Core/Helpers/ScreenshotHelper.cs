using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Helpers
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
            return $"img_{dt.ToString("yyyyMMdd_HHmmssfff")}.{format.ToString().ToLower()}";
        }

        public static DateTime GetScreenDate(string name)
        {
            var dtString = name.Replace("img_", "").Split('.')[0];
            var dt = DateTime.ParseExact(dtString, "yyyyMMdd_HHmmssfff", CultureInfo.InvariantCulture);
            return dt;
        }

        public static string SaveScreenshot(string screenPath, byte[] screenBytes, DateTime creationTime)
        {
            using (var image = Image.FromStream(new MemoryStream(screenBytes)))
            {
                return SaveScreenshot(screenPath, new Bitmap(image), creationTime);
            }
        }

        public static string SaveScreenshot(string screenPath, Image img, DateTime creationTime)
        {
            return SaveScreenshot(screenPath, new Bitmap(img), creationTime);
        }

        public static string SaveScreenshot(string screenPath, Bitmap screen, DateTime creationTime)
        {
            var format = ImageFormat.Png;
            Paths.Create(screenPath);
            creationTime = creationTime.Equals(default(DateTime)) ? DateTime.Now : creationTime;
            var screenName = GetScreenName(creationTime);
            var file = Path.Combine(screenPath, screenName);

            screen.Save(file, format);

            var fileInfo = new FileInfo(file);
            fileInfo.Refresh();
            fileInfo.CreationTime = creationTime;

            return screenName;
        }
    }
}
