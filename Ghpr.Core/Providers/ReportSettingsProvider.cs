using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Providers
{
    internal static class ReportSettingsProvider
    {
        public static void Save(this IReportSettings reportSettings, ILocationsProvider locationsProvider)
        {
            var folder = locationsProvider.SrcPath;
            var serializer = new JsonSerializer();
            folder.Create();
            var fullPath = Path.Combine(folder, Paths.Files.ReportSettings);
            if (!File.Exists(fullPath))
            {
                using (var file = File.CreateText(fullPath))
                {
                    serializer.Serialize(file, reportSettings);
                }
            }
        }

        public static IReportSettings LoadReportSettings(ILocationsProvider locationsProvider)
        {
            IReportSettings settings;
            var folder = locationsProvider.SrcPath;
            var serializer = new JsonSerializer();
            var fullPath = Path.Combine(folder, Paths.Files.ReportSettings);    
            using (var file = File.OpenText(fullPath))
            {
                settings = (IReportSettings)serializer.Deserialize(file, typeof(ReportSettings));
            }
            return settings;
        }
    }
}