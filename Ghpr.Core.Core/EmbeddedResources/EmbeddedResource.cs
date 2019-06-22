using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.EmbeddedResources
{
    public class EmbeddedResource : IEmbeddedResource
    {
        public string FileName { get; set; }
        public ResourceType Type { get; set; }
        public string SearchQuery { get; set; }
        public string RelativePath { get; set; }
        public bool AlwaysReplaceExisting { get; set; }

        public EmbeddedResource(string fileName, ResourceType type, string relativePath, string searchQuery, bool alwaysReplaceExisting)
        {
            FileName = fileName;
            Type = type;
            RelativePath = relativePath;
            SearchQuery = searchQuery;
            AlwaysReplaceExisting = alwaysReplaceExisting;
        }
    }
}