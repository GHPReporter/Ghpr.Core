using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Interfaces
{
    public interface IReporter
    {
        ReportSettingsDto ReportSettings { get; }
        ProjectSettings ReporterSettings { get; }
        bool TestRunStarted { get; }

        IRunDtoRepository RunRepository { get; }
        ITestRunsRepository TestRunsRepository { get; }
        ITestRunDtoProcessor TestRunProcessor { get; }
        IReportCleanUpProcessor ReportCleanUpProcessor { get; }
        IDataWriterService DataWriterService { get; }
        IDataReaderService DataReaderService { get; }
        IActionHelper Action { get; }
        ITestDataProvider TestDataProvider { get; }
        ILogger Logger { get; }

        void RunStarted();
        void SetRunName(string runName);
        void RunFinished();

        void TestStarted(TestRunDto testRun);
        void AddCompleteTestRun(TestRunDto testRun, TestOutputDto testOutputDto);
        void TestFinished(TestRunDto testRun, TestOutputDto testOutputDto);

        void SaveScreenshot(byte[] screenshotBytes, string format);
        void SetTestDataProvider(ITestDataProvider testDataProvider);

        void GenerateFullReport(List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns);
        void GenerateFullReport(List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns, DateTime start, DateTime finish);

        void CleanUpJob();

        void TearDown();
    }
}