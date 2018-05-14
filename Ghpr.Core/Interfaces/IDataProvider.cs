namespace Ghpr.Core.Interfaces
{
    public interface IDataProvider
    {
        IReporterSettings ReporterSettings { get; }
        ILocationsProvider LocationsProvider { get; }

        void SaveTestRun(ITestRun testRun);
        void SaveRun(IRun run);
    }
}