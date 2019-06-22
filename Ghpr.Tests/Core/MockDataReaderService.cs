using System;
using System.Collections.Generic;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Interfaces;
using Ghpr.Core.Core.Settings;

namespace Ghpr.Tests.Core
{
    public class MockDataReaderService : IDataReaderService
    {
        public IDataReaderService GetDataReader()
        {
            return this;
        }

        public void InitializeDataReader(ProjectSettings settings, ILogger logger)
        {
        }

        public ReportSettingsDto GetReportSettings()
        {
            return new ReportSettingsDto(3, 5, "report", "project");
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            return new TestRunDto(Guid.NewGuid(), "Test name", "Full test name");
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            return new TestRunDto(Guid.NewGuid(), "Test name", "Full test name");
        }

        public List<ItemInfoDto> GetTestInfos(Guid testGuid)
        {
            return new List<ItemInfoDto>();
        }

        public List<TestScreenshotDto> GetTestScreenshots(TestRunDto testRunDto)
        {
            return new List<TestScreenshotDto>();
        }

        public TestOutputDto GetTestOutput(TestRunDto testRunDto)
        {
            return new TestOutputDto
            {
                Output = "output",
                SuiteOutput = "suite output",
                TestOutputInfo = new SimpleItemInfoDto {Date = DateTime.Now, ItemName = "item"}
            };
        }

        public RunDto GetRun(Guid runGuid)
        {
            return new RunDto();
        }

        public List<ItemInfoDto> GetRunInfos()
        {
            return new List<ItemInfoDto>();
        }

        public List<TestRunDto> GetTestRunsFromRun(RunDto runDto)
        {
            return new List<TestRunDto>();
        }
    }
}