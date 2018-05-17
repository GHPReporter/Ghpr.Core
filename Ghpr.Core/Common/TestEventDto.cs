using System;

namespace Ghpr.Core.Common
{
    public class TestEventDto
    {
        public string Name { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }

        public double Duration => (Finished - Started).TotalSeconds;
        public string DurationString => (Finished - Started).ToString(@"hh\:mm\:ss\:fff");

        public TestEventDto()
        {
            Name = "";
            Started = default(DateTime);
            Finished = default(DateTime);
        }

        public TestEventDto(string eventName = "", DateTime started = default(DateTime), DateTime finished = default(DateTime))
        {
            Name = eventName;
            Started = started;
            Finished = finished;
        }
    }
}