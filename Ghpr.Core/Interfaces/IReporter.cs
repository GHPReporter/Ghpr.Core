using System;
using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IReporter
    {
        ReportSettingsDto ReportSettings { get; }
        ReporterSettings ReporterSettings { get; }
        bool TestRunStarted { get; }

        IRunDtoRepository RunRepository { get; }
        ITestRunsRepository TestRunsRepository { get; }
        ITestRunDtoProcessor TestRunProcessor { get; }
        IDataService DataService { get; }
        IActionHelper Action { get; }
        ITestDataProvider TestDataProvider { get; }
        ILogger Logger { get; }

        void RunStarted();
        void SetRunName(string runName);
        void RunFinished();

        void TestStarted(TestRunDto testRun);
        void AddCompleteTestRun(TestRunDto testRun, TestOutputDto testOutputDto);
        void TestFinished(TestRunDto testRun, TestOutputDto testOutputDto);

        void SaveScreenshot(byte[] screnshotBytes, string format);

        void GenerateFullReport(List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns);
        void GenerateFullReport(List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns, DateTime start, DateTime finish);

        void TearDown();
    }
}