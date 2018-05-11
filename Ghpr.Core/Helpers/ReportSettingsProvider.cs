using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;
using static Ghpr.Core.Utils.Paths;

namespace Ghpr.Core.Helpers
{
    internal static class ReportSettingsProvider
    {
        public static void Save(this IReportSettings reportSettings, string reportOutputPath)
        {
            var folder = Path.Combine(reportOutputPath, Folders.Src);
            var serializer = new JsonSerializer();
            Create(folder);
            var fullPath = Path.Combine(folder, Files.ReportSettings);
            if (!File.Exists(fullPath))
            {
                using (var file = File.CreateText(fullPath))
                {
                    serializer.Serialize(file, reportSettings);
                }
            }
        }

        public static IReportSettings LoadReportSettings(this string reportOutputPath)
        {
            IReportSettings settings;
            var folder = Path.Combine(reportOutputPath, Folders.Src);
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