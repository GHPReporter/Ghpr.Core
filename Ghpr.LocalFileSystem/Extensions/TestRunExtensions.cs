using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Extensions;
using Ghpr.Core.Helpers;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestRunExtensions
    {
        public static TestRun Update(this TestRun target, TestRun run)
        {
            if (target.TestInfo.Guid.Equals(Guid.Empty))
            {
                target.TestInfo.Guid = target.FullName.ToMd5HashGuid();
            }
            target.Screenshots.AddRange(run.Screenshots.Where(s => !target.Screenshots.Any(ts => ts.Name.Equals(s.Name))));
            target.Events.AddRange(run.Events.Where(e => !target.Events.Any(te => te.Name.Equals(e.Name))));
            return target;
        }

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

        public static TestRun GetTestRun(this List<TestRun> testRuns, TestRun testRun)
        {
            var tr = testRuns.FirstOrDefault(t => t.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
                          ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
                          ?? new TestRun();
            return tr;
        }

        public static TestRun SaveScreenshot(this TestRun testRun, byte[] screenBytes, ILocationsProvider locationsProvider)
        {
            var screenPath = locationsProvider.GetScreenshotPath(testRun.TestInfo.Guid.ToString());
            var screenshotName = ScreenshotHelper.SaveScreenshot(screenPath, screenBytes, DateTime.Now);
            var screenshot = new TestScreenshot(screenshotName);
            testRun.Screenshots.Add(screenshot);
            return testRun;
        }
    }
}