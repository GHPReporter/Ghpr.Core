using System;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestEvent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Started { get; set; }

        [JsonProperty(PropertyName = "finish")]
        public DateTime Finished { get; set; }

        public double Duration => (Finished - Started).TotalSeconds;
        public string DurationString => (Finished - Started).ToString(@"hh\:mm\:ss\:fff");

        public TestEvent()
        {
            Name = "";
            Started = default(DateTime);
            Finished = default(DateTime);
        }

        public TestEvent(string eventName = "", DateTime started = default(DateTime), DateTime finished = default(DateTime))
        {
            Name = eventName;
            Started = started;
            Finished = finished;
        }
    }
}