using System;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Common
{
    public class TestEvent : ITestEvent
    {
        public string Name { get; set; }
        public DateTime Started { get; set; }
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