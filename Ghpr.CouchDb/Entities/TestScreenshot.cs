using System;
using Newtonsoft.Json;

namespace Ghpr.CouchDb.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestScreenshot
    {
        [JsonProperty(PropertyName = "testGuid")]
        public Guid TestGuid { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "base64Data")]
        public string Base64Data { get; set; }
    }
}