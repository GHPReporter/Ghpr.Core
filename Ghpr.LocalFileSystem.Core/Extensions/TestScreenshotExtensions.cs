using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.Core.Providers;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestScreenshotExtensions
    {
        public static string Save(this TestScreenshot testScreenshot, string path)
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

        public static TestScreenshot LoadTestScreenshot(this string fullPath)
        {
            TestScreenshot testScreenshot = null;
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenText(fullPath))
                {
                    var serializer = new JsonSerializer();
                    testScreenshot = (TestScreenshot)serializer.Deserialize(file, typeof(TestScreenshot));
                }
            }
            return testScreenshot;
        }
    }
}