using Ghpr.Core.Enums;

namespace Ghpr.Core.Interfaces
{
    public interface IEmbeddedResource
    {
        string FileName { get; set; }
        ResourceType Type { get; set; }
        string SearchQuery { get; set; }
        string RelativePath { get; set; }
        bool AlwaysReplaceExisting { get; set; }
    }
}