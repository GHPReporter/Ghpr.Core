using System;
using Ghpr.Core.Enums;
using Ghpr.Core.Settings;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core
{
    [TestFixture]
    public class ReporterManagerTests
    {
        [Test]
        public void CreationTest()
        {
            ReporterManager.Initialize(new MockTestDataProvider());
            Assert.AreEqual("C:\\_GHPReporter_Core_Report", ReporterManager.OutputPath);
        }

        [TestCase(TestingFramework.NUnit, "C:\\_GHPReporter_NUnit_Report")]
        [TestCase(TestingFramework.MSTest, "C:\\_GHPReporter_MSTest_Report")]
        [TestCase(TestingFramework.MSTestV2, "C:\\_GHPReporter_MSTestV2_Report")]
        [TestCase(TestingFramework.SpecFlow, "C:\\_GHPReporter_SpecFlow_Report")]
        public void CanCreateByFramework(TestingFramework framework, string outputPath)
        {
            ReporterManager.Initialize(framework, new MockTestDataProvider());
            Assert.AreEqual(outputPath, ReporterManager.OutputPath);
        }

        [Test]
        public void CanCreateWithSettings()
        {
            var s = new ReporterSettings
            {
                RunGuid = Guid.NewGuid().ToString(),
                DataServiceFile = "Ghpr.LocalFileSystem.dll",
                LoggerFile = "",
                OutputPath = @"\\server\folder",
                ProjectName = "cool project",
                RealTimeGeneration = true,
                ReportName = "report name",
                Retention = new RetentionSettings
                {
                    Amount = 3,
                    Till = DateTime.Now
                },
                RunName = "run name",
                RunsToDisplay = 7
            };
            ReporterManager.Initialize(s, new MockTestDataProvider());
            Assert.AreEqual(s.OutputPath, ReporterManager.OutputPath);
        }
    }
}