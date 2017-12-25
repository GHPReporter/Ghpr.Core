using System;
using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IReporter
    {
        void RunStarted();
        void RunFinished();

        void TestStarted(ITestRun testRun);
        void AddCompleteTestRun(ITestRun testRun);
        void TestFinished(ITestRun testRun);

        void GenerateFullReport(List<ITestRun> testRuns);
        void GenerateFullReport(List<ITestRun> testRuns, DateTime start, DateTime finish);
    }
}