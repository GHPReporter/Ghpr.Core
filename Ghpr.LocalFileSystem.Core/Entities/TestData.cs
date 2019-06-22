using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Core.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestData
    {
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "testDataInfo")]
        public SimpleItemInfo TestDataInfo { get; set; }

        [JsonProperty(PropertyName = "actual")]
        public string Actual { get; set; }

        [JsonProperty(PropertyName = "expected")]
        public string Expected { get; set; }
    }
}