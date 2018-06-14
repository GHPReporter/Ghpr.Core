using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IDataService
    {
        void Initialize(ReporterSettings settings, ILogger logger);
        void SaveReportSettings(ReportSettingsDto reportSettings);
        void SaveTestRun(TestRunDto testRun);
        void SaveRun(RunDto run);
        void SaveScreenshot(TestScreenshotDto testScreenshot);
    }
}