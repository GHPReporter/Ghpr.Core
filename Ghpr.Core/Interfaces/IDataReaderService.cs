using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Interfaces
{
    public interface IDataReaderService
    {
        void InitializeDataReader(ReporterSettings settings, ILogger logger);

        ReportSettingsDto GetReportSettings();

        TestRunDto GetLatestTestRun(Guid testGuid);
        TestRunDto GetTestRun(ItemInfoDto testInfo);
        List<TestRunDto> GetTestRuns(Guid testGuid);
        List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo);
        TestOutputDto GetTestOutput(ItemInfoDto testInfo);

        RunDto GetRun(Guid runGuid);
        List<RunDto> GetRuns();
        List<TestRunDto> GetTestRunsFromRun(Guid runGuid);
    }
}