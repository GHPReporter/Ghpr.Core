using System;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestEvent
    {
        [JsonProperty(PropertyName = "eventInfo")]
        public SimpleItemInfo EventInfo { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Started { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finished { get; set; }

        public double Duration => (Finished - Started).TotalSeconds;
        public string DurationString => (Finished - Started).ToString(@"hh\:mm\:ss\:fff");

        public TestEvent()
        {
            Comment = "";
            Started = default(DateTime);
            Finished = default(DateTime);
            EventInfo = new SimpleItemInfo();
        }

        public TestEvent(string comment = "", DateTime started = default(DateTime), DateTime finished = default(DateTime))
        {
            Comment = comment;
            Started = started;
            Finished = finished;
            EventInfo = new SimpleItemInfo();
        }
    }
}