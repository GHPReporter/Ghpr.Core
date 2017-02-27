namespace Ghpr.Core.Interfaces
{
    public interface IReportSettings
    {
        int RunsToDisplay { get; }
        int TestsToDisplay { get; }
        string CoreVersion { get; }
    }
}