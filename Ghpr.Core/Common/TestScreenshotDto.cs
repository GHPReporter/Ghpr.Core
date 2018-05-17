using System;
using Ghpr.Core.Helpers;

namespace Ghpr.Core.Common
{
    public class TestScreenshotDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public TestScreenshotDto()
        {
            var now = DateTime.Now;
            Name = ScreenshotHelper.GetScreenName(now);
            Date = now;
        }

        public TestScreenshotDto(string name)
        {
            Name = name;
            Date = ScreenshotHelper.GetScreenDate(name);
        }

        public TestScreenshotDto(DateTime date)
        {
            Name = ScreenshotHelper.GetScreenName(date);
            Date = date;
        }
    }
}