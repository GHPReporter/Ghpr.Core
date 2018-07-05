using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Services
{
    public class DataWriterService : IDataWriterService
    {
        private readonly IDataWriterService _dataWriterService;
        private ICommonCache _cache;

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
            _dataWriterService.SaveReportSettings(reportSettings);
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput)
        {
            return _dataWriterService.SaveTestRun(testRun, testOutput);
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            _dataWriterService.UpdateTestOutput(testInfo, testOutput);
        }

        public ItemInfoDto SaveRun(RunDto run)
        {
            return _dataWriterService.SaveRun(run);
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            return _dataWriterService.SaveScreenshot(testScreenshot);
        }
    }
}