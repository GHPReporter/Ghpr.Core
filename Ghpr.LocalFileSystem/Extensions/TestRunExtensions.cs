using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Helpers;
using Ghpr.LocalFileSystem.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestRunExtensions
    {
        public static TestRunDto Update(this TestRunDto target, TestRunDto run)
        {
            if (target.TestInfo.Guid.Equals(Guid.Empty))
            {
                target.TestInfo.Guid = target.FullName.ToMd5HashGuid();
            }
            target.Screenshots.AddRange(run.Screenshots.Where(s => !target.Screenshots.Any(ts => ts.Name.Equals(s.Name))));
            target.Events.AddRange(run.Events.Where(e => !target.Events.Any(te => te.Name.Equals(e.Name))));
            return target;
        }

        public static string GetFileName(this TestRunDto testRun)
        {
            return $"test_{testRun.TestInfo.Finish:yyyyMMdd_HHmmssfff}.json";
        }

        public static void Save(this TestRunDto testRun, string path, string name)
        {
            path.Create();
            var fullPath = Path.Combine(path, name.Equals("") ? testRun.GetFileName() : name);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }

        public static TestRunDto LoadTestRun(this string path, string name)
        {
            TestRunDto testRun;
            var fullPath = Path.Combine(path, name);
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                testRun = (TestRunDto)serializer.Deserialize(file, typeof(TestRunDto));
            }
            return testRun;
        }

        public static TestRunDto GetTestRun(this List<TestRunDto> testRuns, TestRunDto testRun)
        {
            var tr = testRuns.FirstOrDefault(t => t.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
                          ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
                          ?? new TestRunDto();
            return tr;
        }

        public static TestRunDto SaveScreenshot(this TestRunDto testRun, byte[] screenBytes, ILocationsProvider locationsProvider)
        {
            var screenPath = locationsProvider.GetScreenshotPath(testRun.TestInfo.Guid.ToString());
            var screenshotName = ScreenshotHelper.SaveScreenshot(screenPath, screenBytes, DateTime.Now);
            var screenshot = new TestScreenshotDto(screenshotName);
            testRun.Screenshots.Add(screenshot);
            return testRun;
        }
    }
}