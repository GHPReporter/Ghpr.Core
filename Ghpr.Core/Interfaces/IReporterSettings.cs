using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IReporterSettings
    {
        ReportSettings ReportSettings { get; }
        string OutputPath { get; }
        string Sprint { get; }
        string RunName { get; }
        string RunGuid { get; }
        bool RealTimeGeneration { get; }
    }
}