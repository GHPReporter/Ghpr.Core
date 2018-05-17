using System;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IRunRepository
    {
        RunDto CurrentRun { get; }
        void OnRunStarted(ReporterSettings settings, DateTime runStartDateTime);
        void OnRunFinished(DateTime runFinishDateTime);
    }
}