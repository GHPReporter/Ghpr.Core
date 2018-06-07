using Newtonsoft.Json;

namespace Ghpr.SerilogToSeqLogger
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LoggerSettings
    {
        [JsonProperty(PropertyName = "endpoint")]
        public string Endpoint { get; set; }
    }
}