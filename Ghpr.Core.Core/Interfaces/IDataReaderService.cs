using System;
using System.Collections.Generic;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Settings;

namespace Ghpr.Core.Core.Interfaces
{
    public interface IDataReaderService
    {
        IDataReaderService GetDataReader();

        void InitializeDataReader(ProjectSettings settings, ILogger logger);

        ReportSettingsDto GetReportSettings();

        TestRunDto GetLatestTestRun(Guid testGuid);
        TestRunDto GetTestRun(ItemInfoDto testInfo);
        List<ItemInfoDto> GetTestInfos(Guid testGuid);
        List<TestScreenshotDto> GetTestScreenshots(TestRunDto testRunDto);
        TestOutputDto GetTestOutput(TestRunDto testRunDto);

        RunDto GetRun(Guid runGuid);
        List<ItemInfoDto> GetRunInfos();
        List<TestRunDto> GetTestRunsFromRun(RunDto runDto);
    }
}