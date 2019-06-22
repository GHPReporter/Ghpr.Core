using System;

namespace Ghpr.Core.Core.Common
{
    public class TestScreenshotDto
    {
        public SimpleItemInfoDto TestScreenshotInfo { get; set; }
        public string Base64Data { get; set; }
        public Guid TestGuid { get; set; }
        public string Format { get; set; }
    }
}