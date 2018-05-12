using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Providers
{
    public abstract class AbstractDataProvider : IDataProvider
    {
        protected AbstractDataProvider(IReporterSettings reporterSettings, ILocationsProvider locationsProvider)
        {
            ReporterSettings = reporterSettings;
            LocationsProvider = LocationsProvider;
        }

        public IReporterSettings ReporterSettings { get; }
        public ILocationsProvider LocationsProvider { get; }

        public abstract void SaveTestRun(IRun run);
        public abstract void SaveRun(ITestRun testRun);
    }
}