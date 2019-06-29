using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Providers
{
    public static class ReportSettingsProvider
    {
        public static string Save(this ReportSettingsDto reportSettings, string folder, string fileName)
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

        public static ReportSettingsDto LoadReportSettings(this string fullPath)
        {
            ReportSettingsDto settings = null;
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenText(fullPath))
                {
                    var serializer = new JsonSerializer();
                    settings = (ReportSettingsDto)serializer.Deserialize(file, typeof(ReportSettingsDto));
                }
            }
            return settings;
        }
    }
}