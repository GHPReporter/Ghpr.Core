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
        void RunFinished();

        void TestStarted(TestRunDto testRun);
        void AddCompleteTestRun(TestRunDto testRun);
        void TestFinished(TestRunDto testRun);

        void SaveScreenshot(byte[] screnshotBytes);

        void GenerateFullReport(List<TestRunDto> testRuns);
        void GenerateFullReport(List<TestRunDto> testRuns, DateTime start, DateTime finish);

        void TearDown();
    }
}