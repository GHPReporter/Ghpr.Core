using System;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemInfo : IItemInfo
    {
        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finish { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        public ItemInfo(IItemInfo ii)
        {
            Guid = ii.Guid;
            Start = ii.Start;
            Finish = ii.Finish;
            FileName = ii.FileName;
        }

        public ItemInfo()
        {
        }
    }
}