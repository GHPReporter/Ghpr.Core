using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RunDto
    {
        [JsonProperty(PropertyName = "testRuns")]
        public List<ItemInfoDto> TestsInfo { get; set; }

        [JsonProperty(PropertyName = "runInfo")]
        public ItemInfoDto RunInfo { get; set; }

        [JsonProperty(PropertyName = "sprint")]
        public string Sprint { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public RunSummaryDto RunSummary { get; set; }
    }
}