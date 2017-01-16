using System;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Helpers
{
    internal static class ReporterHelper
    {
        public static ReporterSettings GetSettingsFromFile(string fileName = "")
        {
            var uri = new Uri(typeof(ReporterSettings).Assembly.CodeBase);
            var settingsPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "",
                fileName.Equals("") ? Names.CoreSettingsFileName : fileName);
            var settings = JsonConvert.DeserializeObject<ReporterSettings>(File.ReadAllText(settingsPath));
            return settings;
        }

        public static string GetSettingsFileName(TestingFramework framework)
        {
            switch (framework)
            {
                case TestingFramework.MSTest:
                    return Names.MSTestSettingsFileName;
                case TestingFramework.NUnit:
                    return Names.NUnitSettingsFileName;
                case TestingFramework.SpecFlow:
                    return Names.SpecFlowSettingsFileName;
                default:
                    return Names.CoreSettingsFileName;
            }
        }
    }
}