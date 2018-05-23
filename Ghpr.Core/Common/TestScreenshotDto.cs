using System;

namespace Ghpr.Core.Common
{
    public class TestScreenshotDto
    {
        public byte[] Data { get; set; }
        public DateTime Date { get; set; }
        public Guid TestGuid { get; set; }
    }
}