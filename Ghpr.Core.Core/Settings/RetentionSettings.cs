using System;
using Newtonsoft.Json;

namespace Ghpr.Core.Core.Settings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RetentionSettings
    {
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }

        [JsonProperty(PropertyName = "till")]
        public DateTime Till { get; set; }
    }
}