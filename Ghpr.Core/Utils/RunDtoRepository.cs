using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class RunDtoRepository : IRunDtoRepository
    {
        public RunDto CurrentRun { get; private set; }
        public Guid RunGuid => CurrentRun.RunInfo.Guid;

        public void OnRunStarted(ReporterSettings settings, DateTime runStartDateTime)
        {
            CurrentRun = new RunDto
            {
                RunInfo = new ItemInfoDto
                {
                    Guid = settings.RunGuid.Equals("") || settings.RunGuid.Equals("null")
                        ? Guid.NewGuid() : Guid.Parse(settings.RunGuid),
                    Start = runStartDateTime
                },
                Name = settings.RunName,
                Sprint = settings.Sprint,
                TestsInfo = new List<ItemInfoDto>(),
                RunSummary = new RunSummaryDto()
            };
        }

        public void OnRunFinished(DateTime runFinishDateTime)
        {
            CurrentRun.RunInfo.Finish = runFinishDateTime;
        }

        public void OnTestFinished(TestRunDto testRun)
        {
            CurrentRun.RunSummary.Total++;
            switch (testRun.TestResult)
            {
                case TestResult.Passed:
                    CurrentRun.RunSummary.Success++;
                    break;
                case TestResult.Failed:
                    CurrentRun.RunSummary.Failures++;
                    break;
                case TestResult.Broken:
                    CurrentRun.RunSummary.Errors++;
                    break;
                case TestResult.Ignored:
                    CurrentRun.RunSummary.Ignored++;
                    break;
                case TestResult.Inconclusive:
                    CurrentRun.RunSummary.Inconclusive++;
                    break;
                case TestResult.Unknown:
                    CurrentRun.RunSummary.Unknown++;
                    break;
                default:
                    CurrentRun.RunSummary.Unknown++;
                    break;
            }
            CurrentRun.TestsInfo.Add(testRun.TestInfo);
        }
    }
}