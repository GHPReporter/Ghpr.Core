using System;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IRunDtoRepository
    {
        RunDto CurrentRun { get; }
        Guid RunGuid { get; }

        void SetRunName(string runName);
        void OnRunStarted(ReporterSettings settings, DateTime runStartDateTime);
        void OnRunFinished(DateTime runFinishDateTime);
        void OnTestFinished(TestRunDto testRun);
    }
}