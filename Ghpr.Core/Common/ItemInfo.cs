using System;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
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

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }
        
        public ItemInfo()
        {
        }

        public ItemInfo(ItemInfo ii)
        {
            Guid = ii.Guid;
            Start = ii.Start;
            Finish = ii.Finish;
            FileName = ii.FileName;
        }
    }
}