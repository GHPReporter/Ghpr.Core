using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Interfaces;
using Ghpr.Core.Core.Settings;
using Ghpr.LocalFileSystem.Core.Entities;
using Ghpr.LocalFileSystem.Core.Extensions;
using Ghpr.LocalFileSystem.Core.Interfaces;
using Ghpr.LocalFileSystem.Core.Mappers;
using Ghpr.LocalFileSystem.Core.Providers;

namespace Ghpr.LocalFileSystem.Core.Services
{
    public class FileSystemDataReaderService : IDataReaderService
    {
        private ILocationsProvider _locationsProvider;
        private ILogger _logger;

        public IDataReaderService GetDataReader()
        {
            return this;
        }

        public void InitializeDataReader(ProjectSettings settings, ILogger logger)
        {
            _locationsProvider = new LocationsProvider(settings.OutputPath);
            _logger = logger;
        }

        public ReportSettingsDto GetReportSettings()
        {
            var reportSettings = _locationsProvider.GetReportSettingsFullPath().LoadReportSettings();
            return reportSettings?.ToDto();
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            var testRuns = GetTestInfos(testGuid);
            return GetTestRun(testRuns.OrderByDescending(t => t.Finish).FirstOrDefault());
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            TestRun test = null;
            if (testInfo != null)
            {
                var testFullPath = _locationsProvider.GetTestFullPath(testInfo.Guid, testInfo.Finish);
                if (File.Exists(testFullPath))
                {
                    test = testFullPath.LoadTestRun();
                }
            }
            return test?.ToDto(test.Output);
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
                var screenPath =
                    _locationsProvider.GetTestScreenshotFullPath(test.TestInfo.Guid, simpleItemInfoDto.Date);
                var screen = screenPath.LoadTestScreenshot()?.ToDto();
                if (screen != null)
                {
                    screens.Add(screen);
                }
            }
            return screens;
        }

        public TestOutputDto GetTestOutput(TestRunDto test)
        {
            TestOutput testOutput = null;
            if (test != null)
            {
                var fullPath = _locationsProvider.GetTestOutputFullPath(test.TestInfo.Guid, test.TestInfo.Finish);
                if (File.Exists(fullPath))
                {
                    testOutput = fullPath.LoadTestOutput();
                }
            }
            return testOutput?.ToDto();
        }

        public RunDto GetRun(Guid runGuid)
        {
            var run = _locationsProvider.RunsFolderPath.LoadRun(NamesProvider.GetRunFileName(runGuid))?.ToDto();
            return run;
        }

        public List<ItemInfoDto> GetRunInfos()
        {
            var runs = _locationsProvider.RunsFolderPath.LoadItemInfos(_locationsProvider.Paths.File.Runs).Select(ii => ii.ToDto()).ToList();
            return runs;
        }

        public List<TestRunDto> GetTestRunsFromRun(RunDto runDto)
        {
            var tests = new List<TestRunDto>();
            foreach (var itemInfoDto in runDto.TestsInfo)
            {
                var test = GetTestRun(itemInfoDto);
                if (test != null)
                {
                    tests.Add(test);
                }
            }
            return tests;
        }
    }
}