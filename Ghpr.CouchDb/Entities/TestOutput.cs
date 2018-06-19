using Newtonsoft.Json;

namespace Ghpr.CouchDb.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestOutputDto
    {
        [JsonProperty(PropertyName = "testOutputInfo")]
        public SimpleItemInfo TestOutputInfo { get; set; }

        [JsonProperty(PropertyName = "output")]
        public string Output { get; set; }

        [JsonProperty(PropertyName = "featureOutput")]
        public string FeatureOutput { get; set; }
    }
}