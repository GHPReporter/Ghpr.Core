using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReportSettings : IReportSettings
    {
        [JsonProperty(PropertyName = "runsToDisplay")]
        public int RunsToDisplay { get; set; }

        [JsonProperty(PropertyName = "testsToDisplay")]
        public int TestsToDisplay { get; set; }

        [JsonProperty(PropertyName = "coreVersion")]
        public string CoreVersion { get; set; }

        public ReportSettings(int runs, int tests)
        {
            RunsToDisplay = runs;
            TestsToDisplay = tests;
            CoreVersion = typeof(ReportSettings).Assembly.GetName().Version.ToString();
        }
    }
}