using System;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;
using static Ghpr.Core.Utils.Paths;

namespace Ghpr.Core.Helpers
{
    internal static class ReporterHelper
    {
        public static ReporterSettings GetSettingsFromFile(string fileName = "")
        {
            var uri = new Uri(typeof(ReporterSettings).Assembly.CodeBase);
            var settingsPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "",
                fileName.Equals("") ? Files.CoreSettings : fileName);
            var settings = JsonConvert.DeserializeObject<ReporterSettings>(File.ReadAllText(settingsPath));
            return settings;
        }

        public static string GetSettingsFileName(TestingFramework framework)
        {
            switch (framework)
            {
                case TestingFramework.MSTest:
                    return Files.MSTestSettings;
                case TestingFramework.MSTestV2:
                    return Files.MSTestV2Settings;
                case TestingFramework.NUnit:
                    return Files.NUnitSettings;
                case TestingFramework.SpecFlow:
                    return Files.SpecFlowSettings;
                default:
                    return Files.CoreSettings;
            }
        }

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