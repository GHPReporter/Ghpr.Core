using System;
using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IDataReaderService
    {
        void Initialize(ReporterSettings settings, ILogger logger);

        TestRunDto GetLatestTestRun(Guid testGuid);
        TestRunDto GetTestRun(ItemInfoDto testInfo);
        List<TestRunDto> GetTestRuns(Guid testGuid);
        List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo);
        List<TestOutputDto> GetTestOutput(ItemInfoDto testInfo);

        RunDto GetRun(Guid runGuid);
        List<RunDto> GetRuns();
        List<TestRunDto> GetTestRunsFromRun(Guid runGuid);
    }
}