using Ghpr.Core.Common;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Interfaces
{
    public interface IDataWriterService
    {
        void InitializeDataWriter(ProjectSettings settings, ILogger logger);
        void SaveReportSettings(ReportSettingsDto reportSettings);
        ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput);
        void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput);
        ItemInfoDto SaveRun(RunDto run);
        SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot);

        void DeleteRun(ItemInfoDto runInfo);
        void DeleteTest(TestRunDto testRun);
        void DeleteTestOutput(TestRunDto testRun, TestOutputDto testOutput);
        void DeleteTestScreenshot(TestRunDto testRun, TestScreenshotDto testScreenshot);
    }
}