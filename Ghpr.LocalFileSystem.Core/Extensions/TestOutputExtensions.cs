using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class TestOutputExtensions
    {
        public static string Save(this TestOutputDto testOutput, string path)
        {
            path.Create();
            var fullPath = Path.Combine(path, NamesProvider.GetTestOutputFileName(testOutput.TestOutputInfo.Date));
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testOutput);
            }
            return fullPath;
        }

        public static TestOutputDto LoadTestOutput(this string fullPath)
        {
            TestOutputDto testOutput;
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                testOutput = (TestOutputDto)serializer.Deserialize(file, typeof(TestOutputDto));
            }
            return testOutput;
        }
    }
}