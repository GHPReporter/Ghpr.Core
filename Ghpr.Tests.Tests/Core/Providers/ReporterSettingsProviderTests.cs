using Ghpr.Core.Enums;
using Ghpr.Core.Providers;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Providers
{
    [TestFixture]
    public class ReporterSettingsProviderTests
    {
        [TestCase(TestingFramework.MSTestV2, "Ghpr.MSTestV2.Settings.json")]
        [TestCase(TestingFramework.MSTest, "Ghpr.MSTest.Settings.json")]
        [TestCase(TestingFramework.NUnit, "Ghpr.NUnit.Settings.json")]
        [TestCase(TestingFramework.SpecFlow, "Ghpr.SpecFlow.Settings.json")]
        public void GetFileNameTests(TestingFramework framework, string expectedFileName)
        {
            var actualFileName = ReporterSettingsProvider.GetFileName(framework);
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestCase(TestingFramework.MSTestV2, "C:\\_GHPReporter_MSTestV2_Report")]
        [TestCase(TestingFramework.MSTest, "C:\\_GHPReporter_MSTest_Report")]
        [TestCase(TestingFramework.NUnit, "C:\\_GHPReporter_NUnit_Report")]
        [TestCase(TestingFramework.SpecFlow, "C:\\_GHPReporter_SpecFlow_Report")]
        public void LoadSettingsByEnum(TestingFramework framework, string expectedLocation)
        {
            var settings = ReporterSettingsProvider.Load(framework);
            Assert.AreEqual(expectedLocation, settings.DefaultSettings.OutputPath);
        }

        [TestCase("Ghpr.MSTestV2.Settings.json", "C:\\_GHPReporter_MSTestV2_Report")]
        [TestCase("Ghpr.MSTest.Settings.json", "C:\\_GHPReporter_MSTest_Report")]
        [TestCase("Ghpr.NUnit.Settings.json", "C:\\_GHPReporter_NUnit_Report")]
        [TestCase("Ghpr.SpecFlow.Settings.json", "C:\\_GHPReporter_SpecFlow_Report")]
        [TestCase("Ghpr.Core.Settings.json", "C:\\_GHPReporter_Core_Report")]
        public void LoadSettingsByFileName(string fileName, string expectedLocation)
        {
            var settings = ReporterSettingsProvider.Load(fileName);
            Assert.AreEqual(expectedLocation, settings.DefaultSettings.OutputPath);
        }
    }
}
