using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.Core.Utils;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Providers
{
    public static class ReportSettingsProvider
    {
        public static void Save(this ReportSettings reportSettings, ILocationsProvider locationsProvider)
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

        public static ReportSettings LoadReportSettings(ILocationsProvider locationsProvider)
        {
            ReportSettings settings;
            var folder = locationsProvider.SrcPath;
            var serializer = new JsonSerializer();
            var fullPath = Path.Combine(folder, Paths.Files.ReportSettings);    
            using (var file = File.OpenText(fullPath))
            {
                settings = (ReportSettings)serializer.Deserialize(file, typeof(ReportSettings));
            }
            return settings;
        }
    }
}