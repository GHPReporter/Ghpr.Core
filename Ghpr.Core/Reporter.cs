using System;
using System.Collections.Generic;
using System.Threading;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core
{
    public class Reporter : IReporter
    {
        public IRunDtoRepository RunRepository { get; internal set; }
        public ITestRunsRepository TestRunsRepository { get; internal set; }
        public ITestRunDtoProcessor TestRunProcessor { get; internal set; }
        public IDataWriterService DataWriterService { get; internal set; }
        public IDataReaderService DataReaderService { get; internal set; }
        public bool TestRunStarted { get; internal set; }
        public ReportSettingsDto ReportSettings { get; internal set; }
        public ReporterSettings ReporterSettings { get; internal set; }
        public IActionHelper Action { get; internal set; }
        public ITestDataProvider TestDataProvider { get; internal set; }
        public ILogger Logger { get; internal set; }

        private void InitializeOnRunStarted(DateTime startDateTime)
        {
            Action.Safe(() =>
            {
                Logger.Debug("Reporter is initializing on run start...");
                RunRepository.OnRunStarted(ReporterSettings, startDateTime);
                TestRunsRepository.OnRunStarted();
                ResourceExtractor.ExtractReportBase(ReporterSettings.OutputPath);
                DataWriterService.SaveReportSettings(ReportSettings);
                Logger.Debug($"Reporter initializing done. Output folder is '{ReporterSettings.OutputPath}'. " +
                             $"Data service file: '{ReporterSettings.DataServiceFile}', Logger file: '{ReporterSettings.LoggerFile}'");
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            Action.Safe(() =>
            {
                RunRepository.OnRunFinished(finishDateTime);
                DataWriterService.SaveRun(RunRepository.CurrentRun);
                Logger.Info($"Report generated at {finishDateTime:yyyy-MM-dd HH:mm:ss.fff}");
            });
        }

        public void RunStarted()
        {
            if (!TestRunStarted)
            {
                var start = DateTime.Now;
                InitializeOnRunStarted(start);
                TestRunStarted = true;
                Logger.Info($"Run started at {start:yyyy-MM-dd HH:mm:ss.fff}");
            }
        }

        public void SetRunName(string runName)
        {
            RunRepository.SetRunName(runName);
        }

        public void RunFinished()
        {
            var finish = DateTime.Now;
            GenerateReport(finish);
            Logger.Info($"Run finished at {finish:yyyy-MM-dd HH:mm:ss.fff}");
        }

        public void TestStarted(TestRunDto testRun)
        {
            Action.Safe(() =>
            {
                TestRunsRepository.AddNewTestRun(testRun);
                Logger.Info($"Test '{testRun.Name}' (Guid: {testRun.TestInfo.Guid}) started");
            });
        }

        public void AddCompleteTestRun(TestRunDto testRun, TestOutputDto testOutputDto)
        {
            OnTestFinish(testRun, testOutputDto);
            Logger.Info($"Test '{testRun.Name}' (Guid: {testRun.TestInfo.Guid}) was added");
        }

        public void TestFinished(TestRunDto testRun, TestOutputDto testOutputDto)
        {
            OnTestFinish(testRun, testOutputDto);
            Logger.Info($"Test '{testRun.Name}' (Guid: {testRun.TestInfo.Guid}) finished");
            if (ReporterSettings.RealTimeGeneration)
            {
                GenerateReport(DateTime.Now);
            }
        }

        public void SetTestDataProvider(ITestDataProvider testDataProvider)
        {
            TestDataProvider = testDataProvider;
        }

        public void SaveScreenshot(byte[] screenshotBytes, string format = "png")
        {
            var now = DateTime.Now;
            while (!TestRunStarted && (DateTime.Now - now).TotalSeconds < 1)
            {
                Thread.Sleep(50);
            }
            var testGuid = TestDataProvider.GetCurrentTestRunGuid();
            var base64String = Convert.ToBase64String(screenshotBytes);
            var testScreenshot = new TestScreenshotDto
            {
                TestGuid = testGuid,
                Base64Data =  base64String,
                TestScreenshotInfo = new SimpleItemInfoDto
                {
                    Date = DateTime.Now,
                    ItemName = string.Empty
                },
                Format = format
            };
            Logger.Info($"Saving screenshot (Test guid: {testScreenshot.TestGuid})");
            DataWriterService.SaveScreenshot(testScreenshot);
        }

        private void OnTestFinish(TestRunDto testDtoWhenFinished, TestOutputDto testOutputDto)
        {
            Action.Safe(() =>
            {
                RunRepository.OnTestFinished(testDtoWhenFinished);
                var testDtoWhenStarted = TestRunsRepository.ExtractCorrespondingTestRun(testDtoWhenFinished);
                var finalTest = TestRunProcessor.Process(testDtoWhenStarted, testDtoWhenFinished, RunRepository.RunGuid);
                Logger.Debug($"Saving test run '{finalTest.Name}' (Guid: {finalTest.TestInfo.Guid})");
                DataWriterService.SaveTestRun(finalTest, testOutputDto);
            });
        }

        public void GenerateFullReport(List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns)
        {
            GenerateFullReport(testRuns, testRuns.GetRunStartDateTime(), testRuns.GetRunFinishDateTime());
        }

        public void GenerateFullReport(List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns, DateTime start, DateTime finish)
        {
            Logger.Debug($"Generating full report from the list of test runs ({testRuns.Count} test runs in total)");
            InitializeOnRunStarted(start);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun.Key, testRun.Value);
            }
            GenerateReport(finish);
            Logger.Debug("Generating full report from the list of test runs: Done");
        }

        public void TearDown()
        {
            Logger.Debug("TearDown called for Reporter");
            Logger.TearDown();
        }
    }
}
