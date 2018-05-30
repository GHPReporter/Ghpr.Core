using System;
using Newtonsoft.Json;

namespace Ghpr.CouchDb.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestScreenshotInfo
    {
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "rev")]
        public string Revision { get; set; }
    }
}