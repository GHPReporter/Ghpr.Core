using System;
using Ghpr.Core.Enums;
using Ghpr.Core.Factories;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using Ghpr.LocalFileSystem.Services;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Factories
{
    [TestFixture]
    public class ReporterFactoryTests
    {
        private readonly ITestDataProvider _provider = new MockTestDataProvider();

        [Test]
        public void CanCreate()
        {
            var r = ReporterFactory.Build(_provider);
            Assert.NotNull(r.ReporterSettings);
            Assert.NotNull(r.Action);
            Assert.NotNull(r.DataReaderService);
            Assert.NotNull(r.DataWriterService);
            Assert.NotNull(r.TestDataProvider);
            Assert.NotNull(r.Logger);
            Assert.IsInstanceOf(typeof(EmptyLogger), r.Logger);
            Assert.IsInstanceOf(typeof(MockTestDataProvider), r.TestDataProvider);
            Assert.IsInstanceOf(typeof(FileSystemDataReaderService), r.DataReaderService.GetDataReader());
            Assert.IsInstanceOf(typeof(FileSystemDataWriterService), r.DataWriterService.GetDataWriter());
            Assert.AreEqual("C:\\_GHPReporter_Core_Report", r.ReporterSettings.OutputPath);
            Assert.AreEqual(r.ReporterSettings.RunsToDisplay, r.ReportSettings.RunsToDisplay);
            Assert.AreEqual(r.ReporterSettings.TestsToDisplay, r.ReportSettings.TestsToDisplay);
            Assert.AreEqual(r.ReporterSettings.ProjectName, r.ReportSettings.ProjectName);
            Assert.AreEqual(r.ReporterSettings.ReportName, r.ReportSettings.ReportName);
        }

        [Test]
        public void CanCreateWithSettings()
        {
            var s = new ReporterSettings
            {
                DefaultSettings = new ProjectSettings
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
                }
            };
            var r = ReporterFactory.Build(s, _provider);
            Assert.NotNull(r.ReporterSettings);
            Assert.NotNull(r.Action);
            Assert.NotNull(r.DataReaderService);
            Assert.NotNull(r.DataWriterService);
            Assert.NotNull(r.TestDataProvider);
            Assert.NotNull(r.Logger);
            Assert.IsInstanceOf(typeof(EmptyLogger), r.Logger);
            Assert.IsInstanceOf(typeof(MockTestDataProvider), r.TestDataProvider);
            Assert.IsInstanceOf(typeof(FileSystemDataReaderService), r.DataReaderService.GetDataReader());
            Assert.IsInstanceOf(typeof(FileSystemDataWriterService), r.DataWriterService.GetDataWriter());
            Assert.AreEqual(s.DefaultSettings.ProjectName, r.ReporterSettings.ProjectName);
            Assert.AreEqual(s.DefaultSettings.ReportName, r.ReporterSettings.ReportName);
            Assert.AreEqual(s.DefaultSettings.RunGuid, r.ReporterSettings.RunGuid);
            Assert.AreEqual(s.DefaultSettings.DataServiceFile, r.ReporterSettings.DataServiceFile);
            Assert.AreEqual(s.DefaultSettings.LoggerFile, r.ReporterSettings.LoggerFile);
            Assert.AreEqual(s.DefaultSettings.OutputPath, r.ReporterSettings.OutputPath);
            Assert.AreEqual(s.DefaultSettings.RealTimeGeneration, r.ReporterSettings.RealTimeGeneration);
            Assert.AreEqual(s.DefaultSettings.Retention.Till, r.ReporterSettings.Retention.Till);
            Assert.AreEqual(s.DefaultSettings.Retention.Amount, r.ReporterSettings.Retention.Amount);
            Assert.AreEqual(s.DefaultSettings.RunName, r.ReporterSettings.RunName);
            Assert.AreEqual(s.DefaultSettings.RunsToDisplay, r.ReporterSettings.RunsToDisplay);
            Assert.AreEqual(s.DefaultSettings.OutputPath, r.ReporterSettings.OutputPath);
            Assert.AreEqual(s.DefaultSettings.RunsToDisplay, r.ReportSettings.RunsToDisplay);
            Assert.AreEqual(s.DefaultSettings.TestsToDisplay, r.ReportSettings.TestsToDisplay);
            Assert.AreEqual(s.DefaultSettings.ProjectName, r.ReportSettings.ProjectName);
            Assert.AreEqual(s.DefaultSettings.ReportName, r.ReportSettings.ReportName);
        }

        [TestCase(TestingFramework.NUnit, "C:\\_GHPReporter_NUnit_Report")]
        [TestCase(TestingFramework.MSTest, "C:\\_GHPReporter_MSTest_Report")]
        [TestCase(TestingFramework.MSTestV2, "C:\\_GHPReporter_MSTestV2_Report")]
        [TestCase(TestingFramework.SpecFlow, "C:\\_GHPReporter_SpecFlow_Report")]
        public void CanCreateByFramework(TestingFramework framework, string outputPath)
        {
            var r = ReporterFactory.Build(framework, _provider);
            Assert.NotNull(r.ReporterSettings);
            Assert.NotNull(r.Action);
            Assert.NotNull(r.DataReaderService);
            Assert.NotNull(r.DataWriterService);
            Assert.NotNull(r.TestDataProvider);
            Assert.NotNull(r.Logger);
            Assert.IsInstanceOf(typeof(EmptyLogger), r.Logger);
            Assert.IsInstanceOf(typeof(MockTestDataProvider), r.TestDataProvider);
            Assert.IsInstanceOf(typeof(FileSystemDataReaderService), r.DataReaderService.GetDataReader());
            Assert.IsInstanceOf(typeof(FileSystemDataWriterService), r.DataWriterService.GetDataWriter());
            Assert.AreEqual(outputPath, r.ReporterSettings.OutputPath);
            Assert.AreEqual(r.ReporterSettings.RunsToDisplay, r.ReportSettings.RunsToDisplay);
            Assert.AreEqual(r.ReporterSettings.TestsToDisplay, r.ReportSettings.TestsToDisplay);
            Assert.AreEqual(r.ReporterSettings.ProjectName, r.ReportSettings.ProjectName);
            Assert.AreEqual(r.ReporterSettings.ReportName, r.ReportSettings.ReportName);
        }
    }
}