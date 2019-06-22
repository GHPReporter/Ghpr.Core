using System.IO;
using Ghpr.Core.Core.Extensions;
using Ghpr.LocalFileSystem.Core.Entities;
using Ghpr.LocalFileSystem.Core.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Core.Extensions
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