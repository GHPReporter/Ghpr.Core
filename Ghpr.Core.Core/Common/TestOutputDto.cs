using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestOutputDto
    {
        [JsonProperty(PropertyName = "testOutputInfo")]
        public SimpleItemInfoDto TestOutputInfo { get; set; }

        [JsonProperty(PropertyName = "output")]
        public string Output { get; set; }

        [JsonProperty(PropertyName = "suiteOutput")]
        public string SuiteOutput { get; set; }
    }
}