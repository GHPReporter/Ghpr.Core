using System;
using Ghpr.LocalFileSystem.Providers;
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
            Name = LocationsProvider.GetScreenshotFileName(now);
            Date = now;
        }

        public TestScreenshot(DateTime date)
        {
            Name = LocationsProvider.GetScreenshotFileName(date);
            Date = date;
        }
    }
}