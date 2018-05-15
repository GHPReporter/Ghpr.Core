using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Providers
{
    public abstract class AbstractDataProvider : IDataProvider
    {
        protected AbstractDataProvider(IReporterSettings reporterSettings, ILocationsProvider locationsProvider)
        {
            ReporterSettings = reporterSettings;
            LocationsProvider = locationsProvider;
        }

        public IReporterSettings ReporterSettings { get; }
        public ILocationsProvider LocationsProvider { get; }

        public abstract void SaveReportSettings(IReportSettings reportSettings);
        public abstract void SaveTestRun(ITestRun testRun);
        public abstract void SaveRun(IRun run);
    }
}