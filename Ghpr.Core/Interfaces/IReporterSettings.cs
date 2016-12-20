namespace Ghpr.Core.Interfaces
{
    public interface IReporterSettings
    {
        string OutputPath { get; }
        string Sprint { get; }
        string RunName { get; }
        string RunGuid { get; }
        bool RealTimeGeneration { get; }
    }
}