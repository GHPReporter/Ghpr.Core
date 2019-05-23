using Newtonsoft.Json;

namespace Ghpr.Core.Settings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ReporterSettings
    {
        [JsonProperty(PropertyName = "default")]
        public ProjectSettings DefaultSettings { get; set; }
        
        [JsonProperty(PropertyName = "projects")]
        public SpecificProjectSettings[] Projects { get; set; }
    }
}