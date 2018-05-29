using Newtonsoft.Json;

namespace Ghpr.CouchDb
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CouchDbSettings
    {
        [JsonProperty(PropertyName = "endpoint")]
        public string Endpoint { get; set; }
    }
}