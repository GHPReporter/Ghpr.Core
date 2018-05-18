using System;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class RunDtoRepository : IRunDtoRepository
    {
        public RunDto CurrentRun { get; private set; }
        public Guid RunGuid => CurrentRun.RunInfo.Guid;

        public void OnRunStarted(ReporterSettings settings, DateTime runStartDateTime)
        {
            CurrentRun = new RunDto(settings, runStartDateTime);
        }

        public void OnRunFinished(DateTime runFinishDateTime)
        {
            CurrentRun.RunInfo.Finish = runFinishDateTime;
        }

        public void OnTestFinished(TestRunDto testRun)
        {
            CurrentRun.RunSummary.Total++;
            CurrentRun.RunSummary = CurrentRun.RunSummary.Update(testRun);
            CurrentRun.TestsInfo.Add(testRun.TestInfo);
        }
    }
}