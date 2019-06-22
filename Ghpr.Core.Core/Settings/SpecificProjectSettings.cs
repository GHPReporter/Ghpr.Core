using Newtonsoft.Json;

namespace Ghpr.Core.Core.Settings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SpecificProjectSettings
    {
        [JsonProperty(PropertyName = "pattern")]
        public string Pattern { get; set; }

        [JsonProperty(PropertyName = "settings")]
        public ProjectSettings Settings { get; set; }
    }
}