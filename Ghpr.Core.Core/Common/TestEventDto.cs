using System;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestEventDto
    {
        [JsonProperty(PropertyName = "eventInfo")]
        public SimpleItemInfoDto EventInfo { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Started { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finished { get; set; }

        public double Duration => (Finished - Started).TotalSeconds;
        public string DurationString => (Finished - Started).ToString(@"hh\:mm\:ss\:fff");

        public TestEventDto()
        {
            Comment = "";
            Started = default(DateTime);
            Finished = default(DateTime);
            EventInfo = new SimpleItemInfoDto();
        }

        public TestEventDto(string eventName = "", DateTime started = default(DateTime), DateTime finished = default(DateTime))
        {
            Comment = eventName;
            Started = started;
            Finished = finished;
            EventInfo = new SimpleItemInfoDto();
        }
    }
}