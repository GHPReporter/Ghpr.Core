using System;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class RunRepository : IRunRepository
    {
        public RunDto CurrentRun { get; private set; }

        public void OnRunStarted(ReporterSettings settings, DateTime runStartDateTime)
        {
            CurrentRun = new RunDto(settings, runStartDateTime);
        }

        public void OnRunFinished(DateTime runFinishDateTime)
        {
            CurrentRun.RunInfo.Finish = runFinishDateTime;
        }

        public void OnNewTestRun(TestRunDto testRun)
        {
            CurrentRun.RunSummary.Total++;
            
            var testGuid = testRun.TestInfo.Guid.ToString();

            CurrentRun.RunSummary = CurrentRun.RunSummary.Update(testRun);

            //CurrentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));
        }

        public Guid GetRunGuid()
        {
            return CurrentRun.RunInfo.Guid;
        }
    }
}