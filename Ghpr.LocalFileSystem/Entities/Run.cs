using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Run
    {
        [JsonProperty(PropertyName = "testRuns")]
        public List<ItemInfo> TestRuns { get; set; }

        [JsonProperty(PropertyName = "runInfo")]
        public ItemInfo RunInfo { get; set; }

        [JsonProperty(PropertyName = "sprint")]
        public string Sprint { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public RunSummary RunSummary { get; set; }
    }
}