using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Interfaces;
using Ghpr.Core.Core.Settings;

namespace Ghpr.Core.Core.Services
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

        public IDataWriterService GetDataWriter()
        {
            return _dataWriterService;
        }

        public void InitializeDataWriter(ProjectSettings settings, ILogger logger)
        {
            _dataWriterService.InitializeDataWriter(settings, logger);
            _cache.InitializeDataWriter(settings, logger);
        }

        public void SaveReportSettings(ReportSettingsDto reportSettings)
        {
            _dataWriterService.SaveReportSettings(reportSettings);
            _cache.SaveReportSettings(reportSettings);
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput)
        {
            var res = _dataWriterService.SaveTestRun(testRun, testOutput);
            _cache.SaveTestRun(testRun, testOutput);
            return res;
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            _dataWriterService.UpdateTestOutput(testInfo, testOutput);
            _cache.UpdateTestOutput(testInfo, testOutput);
        }

        public ItemInfoDto SaveRun(RunDto run)
        {
            var res = _dataWriterService.SaveRun(run);
            _cache.SaveRun(run);
            return res;
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            var res = _dataWriterService.SaveScreenshot(testScreenshot);
            _cache.SaveScreenshot(testScreenshot);
            return res;
        }

        public void DeleteRun(ItemInfoDto runGuid)
        {
            _dataWriterService.DeleteRun(runGuid);
            _cache.DeleteRun(runGuid);
        }

        public void DeleteTest(TestRunDto testRun)
        {
            _dataWriterService.DeleteTest(testRun);
            _cache.DeleteTest(testRun);
        }

        public void DeleteTestOutput(TestRunDto testRun, TestOutputDto testOutput)
        {
            _dataWriterService.DeleteTestOutput(testRun, testOutput);
            _cache.DeleteTestOutput(testRun, testOutput);
        }

        public void DeleteTestScreenshot(TestRunDto testRun, TestScreenshotDto testScreenshot)
        {
            _dataWriterService.DeleteTestScreenshot(testRun, testScreenshot);
            _cache.DeleteTestScreenshot(testRun, testScreenshot);
        }
    }
}