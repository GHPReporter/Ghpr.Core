using System;
using System.Collections.Generic;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Run : IRun
    {
        [JsonProperty]
        public List<string> TestRunFiles { get; set; }
        [JsonProperty]
        public Guid Guid { get; }

        public Run(Guid runGuid)
        {
            Guid = runGuid;
        }
    }
}