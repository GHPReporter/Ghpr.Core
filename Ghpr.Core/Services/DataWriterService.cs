using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Services
{
    public class DataWriterService : IDataWriterService
    {
        private readonly IDataWriterService _dataWriterService;
        private readonly ICommonCache _cache;

        public DataWriterService(IDataWriterService dataWriterService, ICommonCache cache)
        {
            _dataWriterService = dataWriterService;
            _cache = cache;
        }
        
        public void InitializeDataWriter(ReporterSettings settings, ILogger logger)
        {
            _dataWriterService.InitializeDataWriter(settings, logger);
            _cache.InitializeDataWriter(settings, logger);
        }

        public void SaveReportSettings(ReportSettingsDto reportSettings)
        {
            _cache.SaveReportSettings(reportSettings);
            _dataWriterService.SaveReportSettings(reportSettings);
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput)
        {
            _cache.SaveTestRun(testRun, testOutput);
            return _dataWriterService.SaveTestRun(testRun, testOutput);
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            _cache.UpdateTestOutput(testInfo, testOutput);
            _dataWriterService.UpdateTestOutput(testInfo, testOutput);
        }

        public ItemInfoDto SaveRun(RunDto run)
        {
            _cache.SaveRun(run);
            return _dataWriterService.SaveRun(run);
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            _cache.SaveScreenshot(testScreenshot);
            return _dataWriterService.SaveScreenshot(testScreenshot);
        }
    }
}