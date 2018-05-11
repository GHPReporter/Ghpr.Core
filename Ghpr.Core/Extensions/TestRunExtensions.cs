using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class TestRunExtensions
    {
        public static ITestRun Update(this ITestRun target, ITestRun run)
        {
            if (target.TestInfo.Guid.Equals(Guid.Empty))
            {
                target.TestInfo.Guid = GuidConverter.ToMd5HashGuid(target.FullName);
            }
            target.Screenshots.AddRange(run.Screenshots.Where(s => !target.Screenshots.Any(ts => ts.Name.Equals(s.Name))));
            target.Events.AddRange(run.Events.Where(e => !target.Events.Any(te => te.Name.Equals(e.Name))));
            return target;
        }

        public static string GetFileName(this ITestRun testRun)
        {
            return testRun.TestInfo.Finish.GetTestName();
        }

        public static void Save(this ITestRun testRun, string path, string name)
        {
            path.Create();
            var fullPath = Path.Combine(path, name.Equals("") ? testRun.GetFileName() : name);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }

        public static ITestRun LoadTestRun(this string path, string name)
        {
            ITestRun testRun;
            var fullPath = Path.Combine(path, name);
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                testRun = (ITestRun)serializer.Deserialize(file, typeof(TestRun));
            }
            return testRun;
        }

        public static ITestRun GetTestRun(this List<ITestRun> testRuns, ITestRun testRun)
        {
            var tr = testRuns.FirstOrDefault(t => t.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
                          ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
                          ?? new TestRun();
            return tr;
        }

        public static ITestRun SaveScreenshot(this ITestRun testRun, byte[] screenBytes, ILocationsProvider locationsProvider)
        {
            var screenPath = locationsProvider.GetScreenshotPath(testRun.TestInfo.Guid.ToString());
            var screenshotName = ScreenshotHelper.SaveScreenshot(screenPath, screenBytes, DateTime.Now);
            var screenshot = new TestScreenshot(screenshotName);
            testRun.Screenshots.Add(screenshot);
            return testRun;
        }
    }
}