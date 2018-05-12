namespace Ghpr.Core.Interfaces
{
    public interface IDataProvider
    {
        IReporterSettings ReporterSettings { get; }
        ILocationsProvider LocationsProvider { get; }

        void SaveTestRun(IRun run);
        void SaveRun(ITestRun testRun);
    }
}