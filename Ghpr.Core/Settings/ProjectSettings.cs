using Newtonsoft.Json;

namespace Ghpr.Core.Settings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectSettings
    {
        [JsonProperty(PropertyName = "projectNamePattern")]
        public string Pattern { get; set; }

        [JsonProperty(PropertyName = "outputPath")]
        public string OutputPath { get; set; }

        [JsonProperty(PropertyName = "reportName")]
        public string ReportName { get; set; }

        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

        [JsonProperty(PropertyName = "runName")]
        public string RunName { get; set; }

        [JsonProperty(PropertyName = "runGuid")]
        public string RunGuid { get; set; }

        [JsonProperty(PropertyName = "realTimeGeneration")]
        public bool RealTimeGeneration { get; set; }

        [JsonProperty(PropertyName = "runsToDisplay")]
        public int RunsToDisplay { get; set; }

        [JsonProperty(PropertyName = "testsToDisplay")]
        public int TestsToDisplay { get; set; }

        [JsonProperty(PropertyName = "retention")]
        public RetentionSettings Retention { get; set; }
    }
}