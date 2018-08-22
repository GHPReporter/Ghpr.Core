using System;
using System.Collections.Generic;
using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;
using Ghpr.LocalFileSystem.Interfaces;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Services
{
    public class FileSystemDataReaderService : IDataReaderService
    {
        private ILocationsProvider _locationsProvider;
        private ILogger _logger;

        public void InitializeDataReader(ReporterSettings settings, ILogger logger)
        {
            _locationsProvider = new LocationsProvider(settings.OutputPath);
            _logger = logger;
        }

        public ReportSettingsDto GetReportSettings()
        {
            throw new NotImplementedException();
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            throw new NotImplementedException();
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public List<TestRunDto> GetTestRuns(Guid testGuid)
        {
            throw new NotImplementedException();
        }

        public List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public TestOutputDto GetTestOutput(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public RunDto GetRun(Guid runGuid)
        {
            throw new NotImplementedException();
        }

        public List<RunDto> GetRuns()
        {
            throw new NotImplementedException();
        }

        public List<TestRunDto> GetTestRunsFromRun(Guid runGuid)
        {
            throw new NotImplementedException();
        }
    }
}