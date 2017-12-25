namespace Ghpr.Core.Interfaces
{
    public interface IReporterSettings
    {
        int RunsToDisplay { get; }
        int TestsToDisplay { get; }
        string OutputPath { get; }
        string Sprint { get; }
        string RunName { get; }
        string RunGuid { get; }
        bool RealTimeGeneration { get; }
    }
}