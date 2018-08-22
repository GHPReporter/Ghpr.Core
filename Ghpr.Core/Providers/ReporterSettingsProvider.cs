using Ghpr.Core.Enums;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Providers
{
    internal static class ReporterSettingsProvider
    {
        public static ReporterSettings Load(TestingFramework testingFramework)
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

        public static ReporterSettings Load(string fileName)
        {
            return fileName.LoadSettingsAs<ReporterSettings>();
        }
    }
}