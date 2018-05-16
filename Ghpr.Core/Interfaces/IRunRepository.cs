using System;

namespace Ghpr.Core.Interfaces
{
    public interface IRunRepository
    {
        IRun CurrentRun { get; }
        void OnRunStarted(IReporterSettings settings, DateTime runStartDateTime);
        void OnRunFinished(DateTime runFinishDateTime);
    }
}