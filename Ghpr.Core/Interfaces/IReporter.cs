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
        ITestRunDtosRepository TestRunDtosRepository { get; }
        ITestRunDtoProcessor TestRunDtoProcessor { get; }
        IDataService DataService { get; }

        void RunStarted();
        void RunFinished();

        void TestStarted(TestRunDto testRun);
        void AddCompleteTestRun(TestRunDto testRun);
        void TestFinished(TestRunDto testRun);

        void GenerateFullReport(List<TestRunDto> testRuns);
        void GenerateFullReport(List<TestRunDto> testRuns, DateTime start, DateTime finish);
    }
}