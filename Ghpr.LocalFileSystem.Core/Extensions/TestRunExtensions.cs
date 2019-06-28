using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestRunExtensions
    {
        public static string Save(this TestRunDto testRun, string path)
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

        public static TestRunDto LoadTestRun(this string fullPath)
        {
            TestRunDto testRun;
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                testRun = (TestRunDto)serializer.Deserialize(file, typeof(TestRunDto));
            }
            return testRun;
        }
    }
}