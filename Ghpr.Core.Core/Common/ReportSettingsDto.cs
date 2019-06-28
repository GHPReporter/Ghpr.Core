using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReportSettingsDto
    {
        [JsonProperty(PropertyName = "runsToDisplay")]
        public int RunsToDisplay { get; set; }

        [JsonProperty(PropertyName = "testsToDisplay")]
        public int TestsToDisplay { get; set; }

        [JsonProperty(PropertyName = "coreVersion")]
        public string CoreVersion { get; set; }

        [JsonProperty(PropertyName = "reportName")]
        public string ReportName { get; set; }

        [JsonProperty(PropertyName = "projectName")]
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