using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RunSummary
    {
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "success")]
        public int Success { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public int Errors { get; set; }

        [JsonProperty(PropertyName = "failures")]
        public int Failures { get; set; }

        [JsonProperty(PropertyName = "inconclusive")]
        public int Inconclusive { get; set; }

        [JsonProperty(PropertyName = "ignored")]
        public int Ignored { get; set; }

    }
}