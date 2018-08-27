using System.Linq;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Processors
{
    public class ReportCleanUpProcessor : IReportCleanUpProcessor
    {
        public void CleanUpReport(RetentionSettings retentionSettings, IDataReaderService reader, IDataWriterService writer, ILogger logger)
        {
            logger.Debug($"Running Clean up job: deleting the date older than {retentionSettings.Till} and leaving {retentionSettings.Amount} runs only");
            var runInfos = reader.GetRunInfos().OrderByDescending(ri => ri.Finish).ToList();
            var runInfosToDelete = runInfos.Skip(retentionSettings.Amount).ToList();
            runInfosToDelete.AddRange(runInfos.Take(retentionSettings.Amount).Where(ri => ri.Finish < retentionSettings.Till));
            foreach (var itemInfoDto in runInfosToDelete)
            {
                var run = reader.GetRun(itemInfoDto.Guid);
                var tests = reader.GetTestRunsFromRun(run);
                foreach (var test in tests)
                {
                    var testOutput = reader.GetTestOutput(test);
                    var screenshots = reader.GetTestScreenshots(test);
                    foreach (var screenshot in screenshots)
                    {
                        writer.DeleteTestScreenshot(test, screenshot);
                    }
                    writer.DeleteTestOutput(test, testOutput);
                    writer.DeleteTest(test);
                }
                writer.DeleteRun(run.RunInfo);
            }
            logger.Debug("Running Clean up job: done.");
        }
    }
}