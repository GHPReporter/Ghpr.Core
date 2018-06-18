using System;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemInfo
    {
        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finish { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public string ItemName { get; set; }
    }
}