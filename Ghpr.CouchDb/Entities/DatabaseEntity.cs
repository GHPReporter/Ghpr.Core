using Newtonsoft.Json;

namespace Ghpr.CouchDb.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DatabaseEntity<T>
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_rev")]
        public string Rev { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}