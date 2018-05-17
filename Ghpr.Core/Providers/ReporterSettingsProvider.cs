﻿using System;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Providers
{
    internal static class ReporterSettingsProvider
    {
        public static ReporterSettingsDto Load(TestingFramework testingFramework)
        {
            string fileName;
            switch (testingFramework)
            {
                case TestingFramework.MSTest:
                    fileName = Paths.Files.MSTestSettings;
                    break;
                case TestingFramework.MSTestV2:
                    fileName = Paths.Files.MSTestV2Settings;
                    break;
                case TestingFramework.NUnit:
                    fileName = Paths.Files.NUnitSettings;
                    break;
                case TestingFramework.SpecFlow:
                    fileName = Paths.Files.SpecFlowSettings;
                    break;
                default:
                    fileName = Paths.Files.CoreSettings;
                    break;
            }
            var settings = Load(fileName);
            return settings;
        }

        public static ReporterSettingsDto Load(string fileName = "")
        {
            var uri = new Uri(typeof(ReporterSettingsDto).Assembly.CodeBase);
            var settingsPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "",
                fileName.Equals("") ? Paths.Files.CoreSettings : fileName);
            var settings = JsonConvert.DeserializeObject<ReporterSettingsDto>(File.ReadAllText(settingsPath));
            return settings;
        }
    }
}