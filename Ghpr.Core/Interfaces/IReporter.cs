using System;
using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IReporter
    {
        void RunStarted();
        void RunFinished();

        void TestStarted(TestRunDto testRun);
        void AddCompleteTestRun(TestRunDto testRun);
        void TestFinished(TestRunDto testRun);

        void GenerateFullReport(List<TestRunDto> testRuns);
        void GenerateFullReport(List<TestRunDto> testRuns, DateTime start, DateTime finish);

        ReportSettingsDto GetReportSettings();
        ReporterSettings GetReporterSettings();
        bool IsTestRunStarted();
    }
}