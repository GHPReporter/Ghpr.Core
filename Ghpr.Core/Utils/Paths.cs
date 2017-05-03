using System.IO;
// ReSharper disable InconsistentNaming

namespace Ghpr.Core.Utils
{
    public static class Paths
    {
        public static void Create(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string GetRelativeTestRunPath(string testGuid, string testFileName)
        {
            return $"{testGuid}\\{testFileName}";
        }

        public static class Names
        {
            public const string ScreenshotKeyTemplate = "ghpr_screenshot_";
        }

        public static class Folders
        {
            public const string Tests = "tests";
            public const string Runs = "runs";
            public const string Img = "img";
            public const string Src = "src";
        }

        public static class Files
        {
            public const string DefaultLog = "GHPReporter.txt";
            public const string CoreSettings = "Ghpr.Core.Settings.json";
            public const string MSTestSettings = "Ghpr.MSTest.Settings.json";
            public const string NUnitSettings = "Ghpr.NUnit.Settings.json";
            public const string SpecFlowSettings = "Ghpr.SpecFlow.Settings.json";
            public const string ReportSettings = "ReportSettings.json";
            public const string Tests = "tests.json";
            public const string Runs = "runs.json";
        }
    }
}