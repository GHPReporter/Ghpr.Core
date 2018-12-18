namespace Ghpr.Core.Common
{
    public class ReportSettingsDto
    {
        public int RunsToDisplay { get; set; }
        public int TestsToDisplay { get; set; }
        public string CoreVersion { get; set; }
        public string ReportName { get; set; }
        public string ProjectName { get; set; }

        public ReportSettingsDto(int runs, int tests, string reportName, string projectName)
        {
            RunsToDisplay = runs;
            TestsToDisplay = tests;
            CoreVersion = typeof(ReportSettingsDto).Assembly.GetName().Version.ToString();
            ReportName = reportName;
            ProjectName = projectName;
        }
    }
}