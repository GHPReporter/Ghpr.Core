using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.LocalFileSystem.Extensions;
using Ghpr.LocalFileSystem.Interfaces;
using Ghpr.LocalFileSystem.Mappers;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Services
{
    public class FileSystemDataService : IDataService
    {
        public FileSystemDataService(ReporterSettings reporterSettings, ILocationsProvider locationsProvider)
        {
            LocationsProvider = locationsProvider;
            ReporterSettings = reporterSettings;
        }

        public ReporterSettings ReporterSettings { get; }
        public ILocationsProvider LocationsProvider { get; }

        public void SaveRun(RunDto runDto)
        {
            var run = runDto.Map();
            run.Save(LocationsProvider.RunsPath);
            run.RunInfo.SaveRunInfo(LocationsProvider);
        }
        
        public void SaveReportSettings(ReportSettingsDto reportSettingsDto)
        {
            var reportSettings = reportSettingsDto.Map();
            reportSettings.Save(LocationsProvider);
        }

        public void SaveTestRun(TestRunDto testRunDto)
        {
            var testRun = testRunDto.Map();
            testRun.Save(LocationsProvider.GetTestPath(testRun.TestInfo.Guid.ToString()));
            testRun.TestInfo.SaveTestInfo(LocationsProvider);
        }
    }
}