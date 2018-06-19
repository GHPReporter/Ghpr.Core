using System;
using Newtonsoft.Json;

namespace Ghpr.CouchDb.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestEvent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "eventInfo")]
        public SimpleItemInfo EventInfo { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Started { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finished { get; set; }
    }
}