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

        [JsonProperty(PropertyName = "summary")]
        public RunSummary Summary { get; set; }

        public Run(Guid runGuid)
        {
            Guid = runGuid;
        }
    }
}