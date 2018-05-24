using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

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

        private void InitializeOnRunStarted(DateTime startDateTime)
        {
            Action.Safe(() =>
            {
                RunRepository.OnRunStarted(ReporterSettings, startDateTime);
                TestRunsRepository.OnRunStarted();
                ResourceExtractor.ExtractReportBase(ReporterSettings.OutputPath);
                DataService.SaveReportSettings(ReportSettings);
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            Action.Safe(() =>
            {
                RunRepository.OnRunFinished(finishDateTime);
                DataService.SaveRun(RunRepository.CurrentRun);
            });
        }

        public void RunStarted()
        {
            if (!TestRunStarted)
            {
                InitializeOnRunStarted(DateTime.Now);
                TestRunStarted = true;
            }
        }

        public void RunFinished()
        {
            GenerateReport(DateTime.Now);
        }

        public void TestStarted(TestRunDto testRun)
        {
            Action.Safe(() =>
            {
                TestRunsRepository.AddNewTestRun(testRun);
            });
        }

        public void AddCompleteTestRun(TestRunDto testRun)
        {
            OnTestFinish(testRun);
        }

        public void TestFinished(TestRunDto testRun)
        {
            OnTestFinish(testRun);
            if (ReporterSettings.RealTimeGeneration)
            {
                GenerateReport(DateTime.Now);
            }
        }

        public void SaveScreenshot(byte[] screenshotBytes)
        {
            var guid = TestDataProvider.GetCurrentTestRunGuid();
            var testScreenshot = new TestScreenshotDto{TestGuid = guid, Data =  screenshotBytes, Date = DateTime.Now};
            DataService.SaveScreenshot(testScreenshot);
        }

        private void OnTestFinish(TestRunDto testDtoWhenFinished)
        {
            Action.Safe(() =>
            {
                RunRepository.OnTestFinished(testDtoWhenFinished);
                var testDtoWhenStarted = TestRunsRepository.ExtractCorrespondingTestRun(testDtoWhenFinished);
                var finalTest = TestRunProcessor.Process(testDtoWhenStarted, testDtoWhenFinished, RunRepository.RunGuid);
                DataService.SaveTestRun(finalTest);
            });
        }

        public void GenerateFullReport(List<TestRunDto> testRuns)
        {
            GenerateFullReport(testRuns, testRuns.GetRunStartDateTime(), testRuns.GetRunFinishDateTime());
        }

        public void GenerateFullReport(List<TestRunDto> testRuns, DateTime start, DateTime finish)
        {
            InitializeOnRunStarted(start);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun);
            }
            GenerateReport(finish);
        }
    }
}
