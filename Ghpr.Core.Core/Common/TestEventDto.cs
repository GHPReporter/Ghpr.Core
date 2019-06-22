using System;

namespace Ghpr.Core.Core.Common
{
    public class TestEventDto
    {
        public SimpleItemInfoDto EventInfo { get; set; }
        public string Comment { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        
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