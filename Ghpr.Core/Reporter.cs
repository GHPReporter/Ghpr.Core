using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;
using Ghpr.Core.Extensions;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Providers;

namespace Ghpr.Core
{
    public class Reporter : IReporter
    {
        private void InitializeReporter(ReporterSettings settings, IDataService dataService)
        {
            _reporterSettings = settings;
            if (settings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(settings.OutputPath),
                    "Reporter Output path must be specified. Please fix your .json settings file.");
            }
            _reportSettings = new ReportSettingsDto(settings.RunsToDisplay, settings.TestsToDisplay);
            _action = new ActionHelper(settings.OutputPath);
            _dataService = dataService;
            _testRunStarted = false;
        }
        
        public Reporter(ReporterSettings settings, IDataService dataService)
        {
            InitializeReporter(settings, dataService);
        }

        public Reporter(IDataService dataService)
        {
            InitializeReporter(ReporterSettingsProvider.Load(), dataService);
        }

        public Reporter(TestingFramework framework, IDataService dataService)
        {
            InitializeReporter(ReporterSettingsProvider.Load(framework), dataService);
        }

        private RunDto _currentRun;
        private List<TestRunDto> _currentTestRuns;
        private static ActionHelper _action;
        private IDataService _dataService;
        private bool _testRunStarted;
        private ReportSettingsDto _reportSettings;
        private ReporterSettings _reporterSettings;
        
        private void InitializeRun(DateTime startDateTime)
        {
            _action.Safe(() =>
            {
                _currentRun = new RunDto(_reporterSettings, startDateTime);
                _currentTestRuns = new List<TestRunDto>();
                ResourceExtractor.ExtractReportBase(_reporterSettings.OutputPath);
                _dataService.SaveReportSettings(_reportSettings);
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            _action.Safe(() =>
            {
                _currentRun.RunInfo.Finish = finishDateTime;
                _dataService.SaveRun(_currentRun);
            });
        }

        public void RunStarted()
        {
            if (!_testRunStarted)
            {
                InitializeRun(DateTime.Now);
                _testRunStarted = true;
            }
        }

        public void RunFinished()
        {
            GenerateReport(DateTime.Now);
        }

        public void TestStarted(TestRunDto testRun)
        {
            _action.Safe(() =>
            {
                _currentTestRuns.Add(testRun);
            });
        }

        public void AddCompleteTestRun(TestRunDto testRun)
        {
            ProcessTest(testRun, true);
        }

        public void TestFinished(TestRunDto testRun)
        {
            ProcessTest(testRun, false);
            if (_reporterSettings.RealTimeGeneration)
            {
                GenerateReport(DateTime.Now);
            }
        }

        private void ProcessTest(TestRunDto testRun, bool isCompleteTestRun)
        {
            _action.Safe(() =>
            {
                _currentRun.RunSummary.Total++;

                var currentTest = _currentTestRuns.GetTestRun(testRun);
                var finalTest = isCompleteTestRun ? testRun : testRun.UpdateWithExistingTest(currentTest);
                if (!isCompleteTestRun)
                {
                    _currentTestRuns.Remove(currentTest);
                }

                _currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                finalTest.RunGuid = _currentRun.RunInfo.Guid;
                finalTest.TestInfo.Finish = finalTest.TestInfo.Finish.Equals(default(DateTime))
                    ? DateTime.Now
                    : finalTest.TestInfo.Finish;
                finalTest.TestDuration = finalTest.TestDuration.Equals(0.0)
                    ? (finalTest.TestInfo.Finish - finalTest.TestInfo.Start).TotalSeconds
                    : finalTest.TestDuration;

                _dataService.SaveTestRun(finalTest);
                //finalTest.Save(_locationsProvider.GetTestPath(testGuid), fileName);
                //
                //_currentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));
                //
                //finalTest.TestInfo.SaveTestInfo(_locationsProvider);
            });
        }

        public void GenerateFullReport(List<TestRunDto> testRuns)
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }
            var runStart = testRuns.OrderBy(t => t.TestInfo.Start).First(t => !t.TestInfo.Start.Equals(default(DateTime))).TestInfo.Start;
            var runFinish = testRuns.OrderByDescending(t => t.TestInfo.Finish).First(t => !t.TestInfo.Start.Equals(default(DateTime))).TestInfo.Finish;
            GenerateFullReport(testRuns, runStart, runFinish);
        }

        public void GenerateFullReport(List<TestRunDto> testRuns, DateTime start, DateTime finish)
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }

            InitializeRun(start);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun);
            }
            GenerateReport(finish);
        }
        
        public ReportSettingsDto GetReportSettings()
        {
            return _reportSettings;
        }

        public ReporterSettings GetReporterSettings()
        {
            return _reporterSettings;
        }

        public bool IsTestRunStarted()
        {
            return _testRunStarted;
        }
    }
}
