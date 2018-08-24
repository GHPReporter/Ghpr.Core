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
        List<ItemInfoDto> GetTestInfos(Guid testGuid);
        List<TestScreenshotDto> GetTestScreenshots(TestRunDto testRunDto);
        TestOutputDto GetTestOutput(TestRunDto testRunDto);

        RunDto GetRun(Guid runGuid);
        List<ItemInfoDto> GetRunInfos();
        List<TestRunDto> GetTestRunsFromRun(Guid runGuid);
    }
}