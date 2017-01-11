using System;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
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
            Name = ScreenshotHelper.GetScreenName(now);
            Date = now;
        }

        public TestScreenshot(string name)
        {
            Name = name;
            Date = ScreenshotHelper.GetScreenDate(name);
        }

        public TestScreenshot(DateTime date)
        {
            Name = ScreenshotHelper.GetScreenName(date);
            Date = date;
        }
    }
}