using System;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RunInfo : IRunInfo
    {
        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finish { get; set; }

        public RunInfo(IRunInfo ri)
        {
            Guid = ri.Guid;
            Start = ri.Start;
            Finish = ri.Finish;
        }

        public RunInfo()
        {
        }
    }
}