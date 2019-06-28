using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestDataDto
    {
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "testDataInfo")]
        public SimpleItemInfoDto TestDataInfo { get; set; }

        [JsonProperty(PropertyName = "actual")]
        public string Actual { get; set; }

        [JsonProperty(PropertyName = "expected")]
        public string Expected { get; set; }
    }
}