using System;
using System.Collections.Generic;
using System.Drawing;
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
        private void InitializeReporter(IReporterSettings settings)
        {
            _reporterSettings = settings;
            if (settings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(settings.OutputPath),
                    "Reporter Output path must be specified. Please fix your .json settings file.");
            }
            _reportSettings = new ReportSettings(settings.RunsToDisplay, settings.TestsToDisplay);
            _action = new ActionHelper(settings.OutputPath);
            _locationsProvider = new LocationsProvider(settings.OutputPath);
            _dataProvider = new FileSystemDataProvider(settings, _locationsProvider);
            _testRunStarted = false;
        }
        
        public Reporter(IReporterSettings settings)
        {
            InitializeReporter(settings);
        }

        public Reporter()
        {
            InitializeReporter(ReporterSettingsProvider.Load());
        }

        public Reporter(TestingFramework framework)
        {
            InitializeReporter(ReporterSettingsProvider.Load(framework));
        }

        private IRun _currentRun;
        private List<ITestRun> _currentTestRuns;
        private static ActionHelper _action;
        private ILocationsProvider _locationsProvider;
        private IDataProvider _dataProvider;
        private bool _testRunStarted;
        private IReportSettings _reportSettings;
        private IReporterSettings _reporterSettings;
        
        private void InitializeRun(DateTime startDateTime)
        {
            _action.Safe(() =>
            {
                _currentRun = new Run(_reporterSettings, startDateTime);
                _currentTestRuns = new List<ITestRun>();
                ResourceExtractor.ExtractReportBase(_reporterSettings.OutputPath);
                _dataProvider.SaveReportSettings(_reportSettings);
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            _action.Safe(() =>
            {
                _currentRun.RunInfo.Finish = finishDateTime;
                _dataProvider.SaveRun(_currentRun);
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

        public void TestStarted(ITestRun testRun)
        {
            _action.Safe(() =>
            {
                testRun.TestInfo.Start = testRun.TestInfo.Start.Equals(default(DateTime))
                    ? DateTime.Now
                    : testRun.TestInfo.Start;
                _currentTestRuns.Add(testRun);
            });
        }

        public void AddCompleteTestRun(ITestRun testRun)
        {
            ProcessTest(testRun, true);
        }

        public void TestFinished(ITestRun testRun)
        {
            ProcessTest(testRun, false);
            if (_reporterSettings.RealTimeGeneration)
            {
                GenerateReport(DateTime.Now);
            }
        }

        private void ProcessTest(ITestRun testRun, bool isCompleteTestRun)
        {
            _action.Safe(() =>
            {
                _currentRun.RunSummary.Total++;

                var currentTest = _currentTestRuns.GetTestRun(testRun);
                var finalTest = isCompleteTestRun ? testRun : testRun.Update(currentTest);
                if (!isCompleteTestRun)
                {
                    _currentTestRuns.Remove(currentTest);
                }

                var testGuid = finalTest.TestInfo.Guid.ToString();
                var fileName = testRun.GetFileName();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                finalTest.TestInfo.FileName = fileName;
                finalTest.RunGuid = _currentRun.RunInfo.Guid;
                finalTest.TestInfo.Finish = finalTest.TestInfo.Finish.Equals(default(DateTime))
                    ? DateTime.Now
                    : finalTest.TestInfo.Finish;
                finalTest.TestDuration = finalTest.TestDuration.Equals(0.0)
                    ? (finalTest.TestInfo.Finish - finalTest.TestInfo.Start).TotalSeconds
                    : finalTest.TestDuration;

                finalTest.Save(_locationsProvider.GetTestPath(testGuid), fileName);
                
                _currentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));

                finalTest.TestInfo.SaveTestInfo(_locationsProvider);
            });
        }

        public void GenerateFullReport(List<ITestRun> testRuns)
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }
            var runStart = testRuns.OrderBy(t => t.TestInfo.Start).First(t => !t.TestInfo.Start.Equals(default(DateTime))).TestInfo.Start;
            var runFinish = testRuns.OrderByDescending(t => t.TestInfo.Finish).First(t => !t.TestInfo.Start.Equals(default(DateTime))).TestInfo.Finish;
            GenerateFullReport(testRuns, runStart, runFinish);
        }

        public void GenerateFullReport(List<ITestRun> testRuns, DateTime start, DateTime finish)
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
        
        public IReportSettings GetReportSettings()
        {
            return _reportSettings;
        }

        public IReporterSettings GetReporterSettings()
        {
            return _reporterSettings;
        }

        public bool IsTestRunStarted()
        {
            return _testRunStarted;
        }
    }
}
