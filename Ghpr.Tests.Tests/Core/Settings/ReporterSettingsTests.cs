using System;
using System.IO;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core.Settings
{
    [TestFixture]
    public class ReporterSettingsTests
    {
        [Test]
        public void LoadedCorrectly()
        {
            var settings = "Ghpr.Core.Tests.Settings.json".LoadSettingsAs<ReporterSettings>().DefaultSettings;
            Assert.AreEqual("Awesome project", settings.ProjectName);
            Assert.AreEqual("Ghpr.LocalFileSystem.dll", settings.DataServiceFile);
            Assert.AreEqual("log.dll", settings.LoggerFile);
            Assert.AreEqual("C:\\_GHPReporter_Core_Report", settings.OutputPath);
            Assert.AreEqual(true, settings.RealTimeGeneration);
            Assert.AreEqual("GHP Report", settings.ReportName);
            Assert.AreEqual(10, settings.Retention.Amount);
            Assert.AreEqual(new DateTime(2018, 8, 20, 10, 15, 42),
                settings.Retention.Till); //"2018-08-20 10:15:42"
            Assert.AreEqual("66e6f6ba-5b35-475a-a617-394696331f28", settings.RunGuid);
            Assert.AreEqual("Awesome run", settings.RunName);
            Assert.AreEqual(5, settings.RunsToDisplay);
            Assert.AreEqual("Sprint name", settings.Sprint);
            Assert.AreEqual(7, settings.TestsToDisplay);
        }

        [Test]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                "Ghpr.Core.Tests.Settings1.json".LoadSettingsAs<ReporterSettings>();
            });
        }
    }
}
