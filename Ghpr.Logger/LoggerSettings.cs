using Newtonsoft.Json;

namespace Ghpr.Logger
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LoggerSettings
    {
        [JsonProperty(PropertyName = "outputPath")]
        public string OutputPath { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "logLevel")]
        public string LogLevel { get; set; }
    }
}