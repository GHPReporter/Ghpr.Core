using System;

namespace Ghpr.Core.Common
{
    public class TestScreenshotDto
    {
        public string Base64Data { get; set; }
        public DateTime Date { get; set; }
        public Guid TestGuid { get; set; }
    }
}