using System;

namespace Ghpr.Core.Common
{
    public class TestScreenshotDto
    {
        public string ItemName { get; set; }
        public string Base64Data { get; set; }
        public DateTime Date { get; set; }
        public Guid TestGuid { get; set; }
        public string Format { get; set; }
    }
}