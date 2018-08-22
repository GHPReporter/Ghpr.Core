using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Services
{
    public class DataReaderService : IDataReaderService
    {
        private readonly IDataReaderService _dataReaderService;
        private readonly ICommonCache _cache;

        public DataReaderService(IDataReaderService dataReaderService, ICommonCache cache)
        {
            _dataReaderService = dataReaderService;
            _cache = cache;
        }

        public void InitializeDataReader(ReporterSettings settings, ILogger logger)
        {
            _dataReaderService.InitializeDataReader(settings, logger);
            _cache.InitializeDataWriter(settings, logger);
        }

        public ReportSettingsDto GetReportSettings()
        {
            return _cache.GetReportSettings() 
                   ?? _dataReaderService.GetReportSettings();
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            return _cache.GetLatestTestRun(testGuid) 
                   ?? _dataReaderService.GetLatestTestRun(testGuid);
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            return _cache.GetTestRun(testInfo) 
                   ?? _dataReaderService.GetTestRun(testInfo);
        }

        public List<TestRunDto> GetTestRuns(Guid testGuid)
        {
            return _cache.GetTestRuns(testGuid) 
                   ?? _dataReaderService.GetTestRuns(testGuid);
        }

        public List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo)
        {
            return _cache.GetTestScreenshots(testInfo) 
                   ?? _dataReaderService.GetTestScreenshots(testInfo);
        }

        public TestOutputDto GetTestOutput(ItemInfoDto testInfo)
        {
            return _cache.GetTestOutput(testInfo) 
                   ?? _dataReaderService.GetTestOutput(testInfo);
        }

        public RunDto GetRun(Guid runGuid)
        {
            return _cache.GetRun(runGuid) 
                   ?? _dataReaderService.GetRun(runGuid);
        }

        public List<RunDto> GetRuns()
        {
            return _cache.GetRuns() 
                   ?? _dataReaderService.GetRuns();
        }

        public List<TestRunDto> GetTestRunsFromRun(Guid runGuid)
        {
            return _cache.GetTestRunsFromRun(runGuid) 
                   ?? _dataReaderService.GetTestRunsFromRun(runGuid);
        }
    }
}