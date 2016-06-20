using System;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestScreenshot : ITestScreenshot
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        public TestScreenshot()
        {
            var now = DateTime.Now;
            Name = Taker.GetScreenName(now);
            Date = now;
        }

        public TestScreenshot(DateTime date)
        {
            Name = Taker.GetScreenName(date);
            Date = date;
        }
    }
}