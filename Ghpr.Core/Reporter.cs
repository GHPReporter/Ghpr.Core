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
        public IDataService DataService { get; internal set; }
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
                DataService.SaveReportSettings(ReportSettings);
                Logger.Debug($"Reporter initializing done. Output folder is '{ReporterSettings.OutputPath}'. " +
                             $"Data service file: '{ReporterSettings.DataServiceFile}', Logger file: '{ReporterSettings.LoggerFile}'");
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            Action.Safe(() =>
            {
                RunRepository.OnRunFinished(finishDateTime);
                DataService.SaveRun(RunRepository.CurrentRun);
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

        public void AddCompleteTestRun(TestRunDto testRun)
        {
            OnTestFinish(testRun);
            Logger.Info($"Test '{testRun.Name}' (Guid: {testRun.TestInfo.Guid}) was added");
        }

        public void TestFinished(TestRunDto testRun)
        {
            OnTestFinish(testRun);
            Logger.Info($"Test '{testRun.Name}' (Guid: {testRun.TestInfo.Guid}) finished");
            if (ReporterSettings.RealTimeGeneration)
            {
                GenerateReport(DateTime.Now);
            }
        }

        public void SaveScreenshot(byte[] screenshotBytes)
        {
            var now = DateTime.Now;
            while (!TestRunStarted && (DateTime.Now - now).TotalSeconds < 1)
            {
                Thread.Sleep(50);
            }
            var guid = TestDataProvider.GetCurrentTestRunGuid();
            var base64String = Convert.ToBase64String(screenshotBytes);
            var testScreenshot = new TestScreenshotDto{TestGuid = guid, Base64Data =  base64String, Date = DateTime.Now};
            Logger.Info($"Saving screenshot (Test guid: {testScreenshot.TestGuid})");
            DataService.SaveScreenshot(testScreenshot);
        }

        private void OnTestFinish(TestRunDto testDtoWhenFinished)
        {
            Action.Safe(() =>
            {
                RunRepository.OnTestFinished(testDtoWhenFinished);
                var testDtoWhenStarted = TestRunsRepository.ExtractCorrespondingTestRun(testDtoWhenFinished);
                var finalTest = TestRunProcessor.Process(testDtoWhenStarted, testDtoWhenFinished, RunRepository.RunGuid);
                Logger.Debug($"Saving test run '{finalTest.Name}' (Guid: {finalTest.TestInfo.Guid})");
                DataService.SaveTestRun(finalTest);
            });
        }

        public void GenerateFullReport(List<TestRunDto> testRuns)
        {
            GenerateFullReport(testRuns, testRuns.GetRunStartDateTime(), testRuns.GetRunFinishDateTime());
        }

        public void GenerateFullReport(List<TestRunDto> testRuns, DateTime start, DateTime finish)
        {
            Logger.Debug($"Generating full report from the list of test runs ({testRuns.Count} test runs in total)");
            InitializeOnRunStarted(start);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun);
            }
            GenerateReport(finish);
            Logger.Debug("Generating full report from the list of test runs: Done");
        }

        public void TearDown()
        {
            Logger.TearDown();
        }
    }
}
