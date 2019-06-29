using System;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestScreenshotDto
    {
        [JsonProperty(PropertyName = "testScreenshotInfo")]
        public SimpleItemInfoDto TestScreenshotInfo { get; set; }

        [JsonProperty(PropertyName = "base64Data")]
        public string Base64Data { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        public Guid TestGuid { get; set; }
    }
}