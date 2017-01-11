using System;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReporterSettings : IReporterSettings
    {
        [JsonProperty(PropertyName = "guid")]
        public string OutputPath { get; set; }

        [JsonProperty(PropertyName = "sprint")]
        public string Sprint { get; set; }

        [JsonProperty(PropertyName = "runName")]
        public string RunName { get; set; }

        [JsonProperty(PropertyName = "runGuid")]
        public string RunGuid { get; set; }

        [JsonProperty(PropertyName = "realTimeGeneration")]
        public bool RealTimeGeneration { get; set; }

    }
}