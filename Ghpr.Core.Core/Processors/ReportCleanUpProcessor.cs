using System.Linq;
using Ghpr.Core.Core.Interfaces;
using Ghpr.Core.Core.Settings;

namespace Ghpr.Core.Core.Processors
{
    public class ReportCleanUpProcessor : IReportCleanUpProcessor
    {
        private readonly ILogger _logger;
        private readonly IActionHelper _actionHelper;

        public ReportCleanUpProcessor(ILogger logger, IActionHelper actionHelper)
        {
            _logger = logger;
            _actionHelper = actionHelper;
        }

        public void CleanUpReport(RetentionSettings retentionSettings, IDataReaderService reader, IDataWriterService writer)
        {
            _logger.Debug($"Running Clean up job: deleting all runs older than {retentionSettings.Till} and leaving {retentionSettings.Amount} runs only");
            var runInfos = reader.GetRunInfos().OrderByDescending(ri => ri.Finish).ToList();
            var runInfosToDelete = runInfos.Skip(retentionSettings.Amount).ToList();
            runInfosToDelete.AddRange(runInfos.Take(retentionSettings.Amount).Where(ri => ri.Finish < retentionSettings.Till));
            foreach (var itemInfoDto in runInfosToDelete)
            {
                var run = reader.GetRun(itemInfoDto.Guid);
                if (run != null)
                {
                    var tests = reader.GetTestRunsFromRun(run);
                    foreach (var test in tests)
                    {
                        var testOutput = reader.GetTestOutput(test);
                        var screenshots = reader.GetTestScreenshots(test);
                        foreach (var screenshot in screenshots)
                        {
                            _actionHelper.Simple(() =>
                            {
                                writer.DeleteTestScreenshot(test, screenshot);
                            });
                        }
                        _actionHelper.Simple(() =>
                        {
                            writer.DeleteTestOutput(test, testOutput);
                        });
                        _actionHelper.Simple(() =>
                        {
                            writer.DeleteTest(test);
                        });
                    }
                }
                _actionHelper.Simple(() =>
                {
                    writer.DeleteRun(itemInfoDto);
                });
            }
            _logger.Debug("Running Clean up job: done.");
        }
    }
}