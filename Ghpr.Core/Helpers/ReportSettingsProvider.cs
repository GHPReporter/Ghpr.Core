using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;
using static Ghpr.Core.Utils.Paths;

namespace Ghpr.Core.Helpers
{
    internal static class ReportSettingsProvider
    {
        public static void Save(this IReportSettings reportSettings, ILocationsProvider locationsProvider)
        {
            var folder = locationsProvider.SrcPath;
            var serializer = new JsonSerializer();
            folder.Create();
            var fullPath = Path.Combine(folder, Files.ReportSettings);
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
            var fullPath = Path.Combine(folder, Files.ReportSettings);    
            using (var file = File.OpenText(fullPath))
            {
                settings = (IReportSettings)serializer.Deserialize(file, typeof(ReportSettings));
            }
            return settings;
        }
    }
}