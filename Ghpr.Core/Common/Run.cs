using System;
using System.Collections.Generic;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Run : IRun
    {
        [JsonProperty(PropertyName = "testRunFiles")]
        public List<string> TestRunFiles { get; set; }

        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public IRunSummary RunSummary { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finish { get; set; }

        public Run(Guid runGuid)
        {
            Guid = runGuid;
            Name = "";
        }
    }
}