using System;
using Ghpr.Core.Common;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Interfaces
{
    public interface IRunDtoRepository
    {
        RunDto CurrentRun { get; }
        Guid RunGuid { get; }

        void SetRunName(string runName);
        void OnRunStarted(ProjectSettings settings, DateTime runStartDateTime);
        void OnRunFinished(DateTime runFinishDateTime);
        void OnTestFinished(TestRunDto testRun);
    }
}