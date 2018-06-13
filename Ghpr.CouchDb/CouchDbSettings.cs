using Newtonsoft.Json;

namespace Ghpr.CouchDb
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CouchDbSettings
    {
        [JsonProperty(PropertyName = "endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }
    }
}