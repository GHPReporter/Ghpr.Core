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

        [JsonProperty(PropertyName = "runInfo")]
        public ItemInfo RunInfo { get; set; }

        [JsonProperty(PropertyName = "sprint")]
        public string Sprint { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public IRunSummary RunSummary { get; set; }

        public Run(IReporterSettings reporterSettings, DateTime startDateTime)
        {
            RunInfo = new ItemInfo
            {
                Guid = reporterSettings.RunGuid.Equals("") || reporterSettings.RunGuid.Equals("null") 
                ? Guid.NewGuid() : Guid.Parse(reporterSettings.RunGuid),
                Start = startDateTime
            };
            Name = reporterSettings.RunName;
            Sprint = reporterSettings.Sprint;
            TestRunFiles = new List<string>();
            RunSummary = new RunSummary();
        }
    }
}