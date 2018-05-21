using System;
using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Helpers;
using Ghpr.LocalFileSystem.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestRunExtensions
    {
        public static string GetFileName(this TestRun testRun)
        {
            return $"test_{testRun.TestInfo.Finish:yyyyMMdd_HHmmssfff}.json";
        }

        public static void Save(this TestRun testRun, string path)
        {
            path.Create();
            var fullPath = Path.Combine(path, testRun.GetFileName());
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }

        public static TestRun LoadTestRun(this string path, string name)
        {
            TestRun testRun;
            var fullPath = Path.Combine(path, name);
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                testRun = (TestRun)serializer.Deserialize(file, typeof(TestRun));
            }
            return testRun;
        }

        public static TestRun SaveScreenshot(this TestRun testRun, byte[] screenBytes, ILocationsProvider locationsProvider)
        {
            var screenPath = locationsProvider.GetScreenshotPath(testRun.TestInfo.Guid.ToString());
            var screenDate = DateTime.Now;
            ScreenshotHelper.SaveScreenshot(screenPath, screenBytes, screenDate);
            var screenshot = new TestScreenshot(screenDate);
            testRun.Screenshots.Add(screenshot);
            return testRun;
        }
    }
}