using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Services
{
    public class DataReaderService : IDataReaderService
    {
        private readonly IDataReaderService _dataReaderService;

        public DataReaderService(IDataReaderService dataReaderService)
        {
            _dataReaderService = dataReaderService;
        }

        public void Initialize(ReporterSettings settings, ILogger logger)
        {
            _dataReaderService.Initialize(settings, logger);
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            return _dataReaderService.GetLatestTestRun(testGuid);
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            return _dataReaderService.GetTestRun(testInfo);
        }

        public List<TestRunDto> GetTestRuns(Guid testGuid)
        {
            return _dataReaderService.GetTestRuns(testGuid);
        }

        public List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo)
        {
            return _dataReaderService.GetTestScreenshots(testInfo);
        }

        public List<TestOutputDto> GetTestOutput(ItemInfoDto testInfo)
        {
            return _dataReaderService.GetTestOutput(testInfo);
        }

        public RunDto GetRun(Guid runGuid)
        {
            return _dataReaderService.GetRun(runGuid);
        }

        public List<RunDto> GetRuns()
        {
            return _dataReaderService.GetRuns();
        }

        public List<TestRunDto> GetTestRunsFromRun(Guid runGuid)
        {
            return _dataReaderService.GetTestRunsFromRun(runGuid);
        }
    }
}