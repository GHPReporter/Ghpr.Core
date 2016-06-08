using System;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Common
{
    public class TestScreenshot : ITestScreenshot
    {
        public string Name { get; set; }
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