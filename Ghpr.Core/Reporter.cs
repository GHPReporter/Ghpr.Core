using System.IO;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;

namespace Ghpr.Core
{
    public static class Reporter
    {
        public static string OutputPath => Properties.Settings.Default.outputPath;
        public const string Src = "src";

        public static void ExtractReportBase()
        {
            var re = new ResourceExtractor(Path.Combine(OutputPath, Src));
            re.Extract(Resource.All);
        }
    }
}
