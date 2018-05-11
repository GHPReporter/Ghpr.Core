// ReSharper disable InconsistentNaming

namespace Ghpr.Core.Utils
{
    public class Paths
    {
        public Paths()
        {
            Name = new Names();
            Folder = new Folders();
            File = new Files();
        }

        public Names Name { get; }
        public Folders Folder { get; }
        public Files File { get; }

        public class Names
        {
            public static string ScreenshotKeyTemplate = "ghpr_screenshot_";
            public static string TestDataCommentKeyTemplate = "ghpr_test_data_comment_";
            public static string TestDataDateTimeKeyTemplate = "ghpr_test_data_datetime_";
            public static string TestDataActualKeyTemplate = "ghpr_test_data_actual_";
            public static string TestDataExpectedKeyTemplate = "ghpr_test_data_expected_";
        }

        public class Folders
        {
            public string Tests = "tests";
            public string Runs = "runs";
            public string Img = "img";
            public string Src = "src";
        }

        public class Files
        {
            public static string CoreSettings = "Ghpr.Core.Settings.json";
            public static string MSTestSettings = "Ghpr.MSTest.Settings.json";
            public static string MSTestV2Settings = "Ghpr.MSTestV2.Settings.json";
            public static string NUnitSettings = "Ghpr.NUnit.Settings.json";
            public static string SpecFlowSettings = "Ghpr.SpecFlow.Settings.json";
            public static string ReportSettings = "ReportSettings.json";
            public string Tests = "tests.json";
            public string Runs = "runs.json";
        }
    }
}