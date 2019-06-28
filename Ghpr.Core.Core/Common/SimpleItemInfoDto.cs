using System;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SimpleItemInfoDto
    {
        [JsonProperty(PropertyName = "itemName")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
    }
}