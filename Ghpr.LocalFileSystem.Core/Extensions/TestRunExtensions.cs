using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.Core.Providers;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestRunExtensions
    {
        public static string Save(this TestRun testRun, string path)
        {
            path.Create();
            var fullPath = Path.Combine(path, NamesProvider.GetTestRunFileName(testRun.TestInfo.Finish));
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
            return fullPath;
        }

        public static TestRun LoadTestRun(this string fullPath)
        {
            TestRun testRun;
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                testRun = (TestRun)serializer.Deserialize(file, typeof(TestRun));
            }
            return testRun;
        }
    }
}