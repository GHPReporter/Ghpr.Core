using System.IO;
using Ghpr.Core.Core.Extensions;
using Ghpr.LocalFileSystem.Core.Entities;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Core.Providers
{
    public static class ReportSettingsProvider
    {
        public static string Save(this ReportSettings reportSettings, string folder, string fileName)
        {
            var serializer = new JsonSerializer();
            folder.Create();
            var fullPath = Path.Combine(folder, fileName);
            if (!File.Exists(fullPath))
            {
                using (var file = File.CreateText(fullPath))
                {
                    serializer.Serialize(file, reportSettings);
                }
            }
            return fullPath;
        }

        public static ReportSettings LoadReportSettings(this string fullPath)
        {
            ReportSettings settings = null;
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenText(fullPath))
                {
                    var serializer = new JsonSerializer();
                    settings = (ReportSettings)serializer.Deserialize(file, typeof(ReportSettings));
                }
            }
            return settings;
        }
    }
}