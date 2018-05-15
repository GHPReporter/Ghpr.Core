namespace Ghpr.Core.Interfaces
{
    public interface IDataProvider
    {
        IReporterSettings ReporterSettings { get; }
        ILocationsProvider LocationsProvider { get; }

        void SaveReportSettings(IReportSettings reportSettings);
        void SaveTestRun(ITestRun testRun);
        void SaveRun(IRun run);
    }
}