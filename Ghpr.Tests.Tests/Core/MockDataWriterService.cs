using System;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;

namespace Ghpr.Tests.Tests.Core
{
    public class MockDataWriterService : IDataWriterService
    {
        public IDataWriterService GetDataWriter()
        {
            return this;
        }

        public void InitializeDataWriter(ProjectSettings settings, ILogger logger)
        {
        }

        public void SaveReportSettings(ReportSettingsDto reportSettings)
        {
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput)
        {
            return new ItemInfoDto { Guid = Guid.NewGuid(), Start = DateTime.Now, ItemName = "SaveTestRun", Finish = DateTime.Now.AddSeconds(3)};
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
        }

        public ItemInfoDto SaveRun(RunDto run)
        {
            return new ItemInfoDto { Guid = Guid.NewGuid(), Start = DateTime.Now, ItemName = "SaveRun", Finish = DateTime.Now.AddSeconds(3) };
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            return new SimpleItemInfoDto { Date = DateTime.Now, ItemName = "SaveScreenshot" };
        }

        public void DeleteRun(ItemInfoDto runInfo)
        {
        }

        public void DeleteTest(TestRunDto testRun)
        {
        }

        public void DeleteTestOutput(TestRunDto testRun, TestOutputDto testOutput)
        {
        }

        public void DeleteTestScreenshot(TestRunDto testRun, TestScreenshotDto testScreenshot)
        {
        }
    }
}