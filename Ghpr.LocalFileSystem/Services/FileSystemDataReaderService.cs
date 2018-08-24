using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Extensions;
using Ghpr.LocalFileSystem.Interfaces;
using Ghpr.LocalFileSystem.Mappers;
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
            var reportSettings = _locationsProvider.GetReportSettingsFullPath().LoadReportSettings();
            return reportSettings.ToDto();
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            var testRuns = GetTestInfos(testGuid);
            return GetTestRun(testRuns.OrderByDescending(t => t.Finish).First());
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            var test = _locationsProvider.GetTestFullPath(testInfo.Guid, testInfo.Finish).LoadTestRun();
            return test.ToDto(test.Output);
        }

        public List<ItemInfoDto> GetTestInfos(Guid testGuid)
        {
            var test = _locationsProvider.GetTestFolderPath(testGuid)
                .LoadItemInfos(_locationsProvider.Paths.File.Tests).Select(ii => ii.ToDto()).ToList();
            return test;
        }

        public List<TestScreenshotDto> GetTestScreenshots(TestRunDto test)
        {
            var screens = new List<TestScreenshotDto>();
            foreach (var simpleItemInfoDto in test.Screenshots)
            {
                var screen = _locationsProvider.GetTestScreenshotFullPath(test.TestInfo.Guid, simpleItemInfoDto.Date)
                    .LoadTestScreenshot().ToDto();
                screens.Add(screen);
            }
            return screens;
        }

        public TestOutputDto GetTestOutput(TestRunDto test)
        {
            var output = _locationsProvider.GetTestOutputFullPath(test.TestInfo.Guid, test.TestInfo.Finish)
                .LoadTestOutput();
            return output.ToDto();
        }

        public RunDto GetRun(Guid runGuid)
        {
            var run = _locationsProvider.RunsFolderPath.LoadRun(NamesProvider.GetRunFileName(runGuid)).ToDto();
            return run;
        }

        public List<ItemInfoDto> GetRunInfos()
        {
            var runs = _locationsProvider.RunsFolderPath.LoadItemInfos(_locationsProvider.Paths.File.Runs).Select(ii => ii.ToDto()).ToList();
            return runs;
        }

        public List<TestRunDto> GetTestRunsFromRun(Guid runGuid)
        {
            throw new NotImplementedException();
        }
    }
}