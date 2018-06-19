using System;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestScreenshot
    {
        [JsonProperty(PropertyName = "testScreenshotInfo")]
        public SimpleItemInfo TestScreenshotInfo { get; set; }

        [JsonProperty(PropertyName = "base64Data")]
        public string Base64Data { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        public Guid TestGuid { get; set; }
    }
}