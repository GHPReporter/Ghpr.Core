using System;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IDataService
    {
        void Initialize(ReporterSettings settings);

        ReporterSettings ReporterSettings { get; }

        void SaveReportSettings(ReportSettingsDto reportSettings);
        void SaveTestRun(TestRunDto testRun);
        void SaveRun(RunDto run);
        void SaveScreenshot(TestScreenshotDto testScreenshot);
    }
}