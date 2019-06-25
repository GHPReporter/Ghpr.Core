using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
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

        [JsonProperty(PropertyName = "unknown")]
        public int Unknown { get; set; }

        public RunSummary(int total = 0, int success = 0, int errors = 0, int failures = 0, int inconclusive = 0, int ignored = 0, int unknown = 0)
        {
            Total = total;
            Success = success;
            Errors = errors;
            Failures = failures;
            Inconclusive = inconclusive;
            Ignored = ignored;
            Unknown = unknown;
        }
    }
}