using Ghpr.Core.Core.Enums;
using Ghpr.Core.Core.Settings;
using Ghpr.Core.Core.Utils;

namespace Ghpr.Core.Core.Providers
{
    public static class ReporterSettingsProvider
    {
        public static string GetFileName(TestingFramework testingFramework)
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
            return fileName;
        }

        public static ReporterSettings Load(TestingFramework testingFramework)
        {
            var settings = Load(GetFileName(testingFramework));
            return settings;
        }

        public static ReporterSettings Load(string fileName)
        {
            return fileName.LoadSettingsAs<ReporterSettings>();
        }
    }
}