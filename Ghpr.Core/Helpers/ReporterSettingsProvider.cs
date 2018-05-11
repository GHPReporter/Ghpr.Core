using System;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;
using static Ghpr.Core.Utils.Paths;

namespace Ghpr.Core.Helpers
{
    internal static class ReporterSettingsProvider
    {
        public static IReporterSettings Load(TestingFramework testingFramework)
        {
            string fileName;
            switch (testingFramework)
            {
                case TestingFramework.MSTest:
                    fileName = Files.MSTestSettings;
                    break;
                case TestingFramework.MSTestV2:
                    fileName = Files.MSTestV2Settings;
                    break;
                case TestingFramework.NUnit:
                    fileName = Files.NUnitSettings;
                    break;
                case TestingFramework.SpecFlow:
                    fileName = Files.SpecFlowSettings;
                    break;
                default:
                    fileName = Files.CoreSettings;
                    break;
            }
            var settings = Load(fileName);
            return settings;
        }

        public static IReporterSettings Load(string fileName = "")
        {
            var uri = new Uri(typeof(ReporterSettings).Assembly.CodeBase);
            var settingsPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "",
                fileName.Equals("") ? Files.CoreSettings : fileName);
            var settings = JsonConvert.DeserializeObject<ReporterSettings>(File.ReadAllText(settingsPath));
            return settings;
        }
    }
}