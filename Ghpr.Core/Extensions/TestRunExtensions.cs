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

        public static ITestRun UpdateWith(this ITestRun startTestRun, ITestRun finishTestRun)
        {
            if (finishTestRun.Guid.Equals(Guid.Empty))
            {
                finishTestRun.Guid = GuidConverter.ToMd5HashGuid(finishTestRun.FullName);
            }
            startTestRun.Guid = finishTestRun.Guid.Equals(Guid.Empty) ? startTestRun.Guid : finishTestRun.Guid;
            startTestRun.Name = finishTestRun.Name.Equals("") ? startTestRun.Name : finishTestRun.Name;
            startTestRun.FullName = finishTestRun.FullName.Equals("") ? startTestRun.FullName : finishTestRun.FullName;
            if (finishTestRun.Events.Any())
            {
                startTestRun.Events.AddRange(finishTestRun.Events);
            }
            if (finishTestRun.Screenshots.Any())
            {
                startTestRun.Screenshots.AddRange(finishTestRun.Screenshots);
            }
            startTestRun.TestStackTrace = finishTestRun.TestStackTrace;
            startTestRun.TestMessage = finishTestRun.TestMessage;
            startTestRun.Result = finishTestRun.Result;
            if (startTestRun.DateTimeFinish.Equals(default(DateTime)))
            {
                startTestRun.DateTimeFinish = DateTime.Now;
            }
            return startTestRun;
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
        
        public static ITestRun GetTest(this List<ITestRun> testRuns, ITestRun testRun)
        {
            return testRuns.FirstOrDefault(t => t.Guid.Equals(testRun.Guid) && !t.Guid.Equals(Guid.Empty))
                ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
                ?? testRuns.FirstOrDefault(t => t.Name.Equals(testRun.Name)) 
                ?? new TestRun();
        }
    }
}