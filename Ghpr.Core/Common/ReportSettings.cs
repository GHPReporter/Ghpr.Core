using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReportSettings
    {
        [JsonProperty(PropertyName = "runsToDisplay")]
        public int RunsToDisplay { get; set; }

        [JsonProperty(PropertyName = "runGuid")]
        public int TestsToDisplay { get; set; }

    }
}