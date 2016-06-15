using System;
using System.IO;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class TestRunExtensions
    {
        public static ITestRun SetFinishDateTime(this ITestRun testRun, DateTime finishDateTime)
        {
            testRun.DateTimeFinish = finishDateTime;
            return testRun;
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
    }
}