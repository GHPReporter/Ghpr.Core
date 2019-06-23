using System;

namespace Ghpr.LocalFileSystem.Providers
{
    public static class NamesProvider
    {
        public static string GetTestRunFileName(DateTime finishDateTime)
        {
            return $"test_{finishDateTime:yyyyMMdd_HHmmssfff}.json";
        }

        public static string GetTestOutputFileName(DateTime finishDateTime)
        {
            return $"test_output_{finishDateTime:yyyyMMdd_HHmmssfff}.json";
        }

        public static string GetRunFileName(Guid runGuid)
        {
            var fileName = $"run_{runGuid.ToString().ToLower()}.json";
            return fileName;
        }

        public static string GetScreenshotFileName(DateTime creationDateTime)
        {
            return $"img_{creationDateTime:yyyyMMdd_HHmmssfff}.json";
        }

    }
}