using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class TestRunExtensions
    {
        public static ITestRun SetFinishDateTime(this ITestRun testRun, DateTime finishDateTime = default(DateTime))
        {
            testRun.DateTimeFinish = finishDateTime.Equals(default(DateTime)) ? DateTime.Now : finishDateTime;
            return testRun;
        }

        public static ITestRun TakeScreenshot(this ITestRun testRun, string testPath, bool takeScreenshot)
        {
            if (takeScreenshot && testRun.FailedOrBroken)
            {
                var date = DateTime.Now;
                var s = new TestScreenshot(date);
                Taker.TakeScreenshot(Path.Combine(testPath, "img"), date);
                testRun.Screenshots.Add(s);
            }
            return testRun;
        }

        public static ITestRun UpdateWith(this ITestRun targetTestRun, ITestRun testRunResult)
        {
            targetTestRun.Guid = testRunResult.Guid.Equals(Guid.Empty) ? targetTestRun.Guid : testRunResult.Guid;
            targetTestRun.Name = testRunResult.Name.Equals("") ? targetTestRun.Name : testRunResult.Name;
            targetTestRun.FullName = testRunResult.FullName.Equals("") ? targetTestRun.FullName : testRunResult.FullName;
            targetTestRun.Events.AddRange(testRunResult.Events);
            targetTestRun.Screenshots.AddRange(testRunResult.Screenshots);
            targetTestRun.TestStackTrace = testRunResult.TestStackTrace;
            targetTestRun.TestMessage = testRunResult.TestMessage;
            targetTestRun.Result = testRunResult.Result;
            return testRunResult;
        }

        public static string GetFileName(this ITestRun testRun)
        {
            return testRun.DateTimeFinish.GetTestName();
        }
        
        public static void Save(this ITestRun testRun, string path, string name = "")
        {
            if (name.Equals(""))
            {
                name = testRun.GetFileName();
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullPath = Path.Combine(path, name);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }

        public static ITestRun GetTest(this List<ITestRun> testRuns, string guid, string name = "", string fullName = "")
        {
            return testRuns.FirstOrDefault(t => t.Guid.Equals(Guid.Parse(guid)) && !t.Guid.Equals(Guid.Empty))
                ?? testRuns.FirstOrDefault(t => t.FullName.Equals(fullName))
                ?? testRuns.FirstOrDefault(t => t.Name.Equals(name));
        }

        public static ITestRun GetTest(this List<ITestRun> testRuns, ITestRun testRun)
        {
            return testRuns.FirstOrDefault(t => t.Guid.Equals(testRun.Guid) && !t.Guid.Equals(Guid.Empty))
                ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
                ?? testRuns.FirstOrDefault(t => t.Name.Equals(testRun.Name));
        }
    }
}