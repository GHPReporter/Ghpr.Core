using System;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Core.Tests
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void TestMethod()
        {
            var settings = "Ghpr.Core.Settings.json".LoadSettingsAs<ReporterSettings>();
            Assert.AreEqual("Awesome project", settings.ProjectName);
            Assert.AreEqual("Ghpr.LocalFileSystem.dll", settings.DataServiceFile);
            Assert.AreEqual("log.dll", settings.LoggerFile);
            Assert.AreEqual("C:\\_GHPReporter_Core_Report", settings.OutputPath);
            Assert.AreEqual(true, settings.RealTimeGeneration);
            Assert.AreEqual("GHP Report", settings.ReportName);
            Assert.AreEqual(10, settings.Retention.Amount);
            Assert.AreEqual(new DateTime(2018, 8, 20, 10, 15, 42), settings.Retention.Till);//"2018-08-20 10:15:42"
            Assert.AreEqual("66e6f6ba-5b35-475a-a617-394696331f28", settings.RunGuid);
            Assert.AreEqual("Awesome run", settings.RunName);
            Assert.AreEqual(5, settings.RunsToDisplay);
            Assert.AreEqual("Sprint name", settings.Sprint);
            Assert.AreEqual(7, settings.TestsToDisplay);
        }
    }
}
