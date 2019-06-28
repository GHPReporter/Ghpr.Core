using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestScreenshotExtensions
    {
        public static string Save(this TestScreenshotDto testScreenshot, string path)
        {
            path.Create();
            var fullPath = Path.Combine(path, NamesProvider.GetScreenshotFileName(testScreenshot.TestScreenshotInfo.Date));
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testScreenshot);
            }
            var fileInfo = new FileInfo(fullPath);
            fileInfo.Refresh();
            fileInfo.CreationTime = testScreenshot.TestScreenshotInfo.Date;
            return fullPath;
        }

        public static TestScreenshotDto LoadTestScreenshot(this string fullPath)
        {
            TestScreenshotDto testScreenshot = null;
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenText(fullPath))
                {
                    var serializer = new JsonSerializer();
                    testScreenshot = (TestScreenshotDto)serializer.Deserialize(file, typeof(TestScreenshotDto));
                }
            }
            return testScreenshot;
        }
    }
}