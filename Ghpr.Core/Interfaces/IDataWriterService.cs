using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IDataWriterService
    {
        void InitializeDataWriter(ReporterSettings settings, ILogger logger);
        void SaveReportSettings(ReportSettingsDto reportSettings);
        ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput);
        void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput);
        ItemInfoDto SaveRun(RunDto run);
        SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot);
    }
}