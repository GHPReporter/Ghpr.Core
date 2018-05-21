using System;
using Ghpr.Core.Helpers;
using Ghpr.LocalFileSystem.Helpers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestScreenshot
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

        public TestScreenshot(DateTime date)
        {
            Name = ScreenshotHelper.GetScreenName(date);
            Date = date;
        }
    }
}