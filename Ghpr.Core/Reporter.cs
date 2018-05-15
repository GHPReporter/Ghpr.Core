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
        private ITestRun _currentTestRun;
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
                _reportSettings.Save(_locationsProvider);
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
                var startDateTime = DateTime.Now;
                testRun.TestInfo.Start = testRun.TestInfo.Start.Equals(default(DateTime))
                    ? startDateTime
                    : testRun.TestInfo.Start;
                _currentTestRuns.Add(testRun);
                _currentTestRun = testRun;
            });
        }

        public void SaveScreenshot(Bitmap screen)
        {
            _action.Safe(() =>
            {
                var testGuid = _currentTestRun.TestInfo.Guid.ToString();
                var date = DateTime.Now;
                var s = new TestScreenshot(date);
                ScreenshotHelper.SaveScreenshot(_locationsProvider.GetScreenshotPath(testGuid), screen, date);
                _currentTestRun.Screenshots.Add(s);
                _currentTestRuns.First(
                    tr => tr.TestInfo.Guid.Equals(_currentTestRun.TestInfo.Guid))
                    .Screenshots.Add(s);
            });
        }
        
        public void AddCompleteTestRun(ITestRun testRun)
        {
            _action.Safe(() =>
            {
                _currentRun.RunSummary.Total++;

                var finishDateTime = testRun.TestInfo.Finish;
                var testGuid = testRun.TestInfo.Guid.ToString();
                var fileName = testRun.GetFileName();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(testRun);

                testRun.TestInfo.FileName = fileName;
                testRun.RunGuid = _currentRun.RunInfo.Guid;
                testRun.TestInfo.Finish = testRun.TestInfo.Finish.Equals(default(DateTime))
                    ? finishDateTime
                    : testRun.TestInfo.Finish;
                testRun.TestDuration = testRun.TestDuration.Equals(0.0)
                    ? (testRun.TestInfo.Finish - testRun.TestInfo.Start).TotalSeconds
                    : testRun.TestDuration;

                testRun.Save(_locationsProvider.GetTestPath(testGuid), fileName);
                _currentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));

                testRun.TestInfo.SaveTestInfo(_locationsProvider);
            });
        }

        public void TestFinished(ITestRun testRun)
        {
            _action.Safe(() =>
            {
                _currentRun.RunSummary.Total++;

                var finishDateTime = DateTime.Now;
                var currentTest = _currentTestRuns.GetTestRun(testRun);
                var finalTest = testRun.Update(currentTest);
                var testGuid = finalTest.TestInfo.Guid.ToString();
                var fileName = testRun.GetFileName();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                finalTest.TestInfo.FileName = fileName;
                finalTest.RunGuid = _currentRun.RunInfo.Guid;
                finalTest.TestInfo.Finish = finalTest.TestInfo.Finish.Equals(default(DateTime))
                    ? finishDateTime
                    : finalTest.TestInfo.Finish;
                finalTest.TestDuration = finalTest.TestDuration.Equals(0.0)
                    ? (finalTest.TestInfo.Finish - finalTest.TestInfo.Start).TotalSeconds
                    : finalTest.TestDuration;

                finalTest.Save(_locationsProvider.GetTestPath(testGuid), fileName);
                _currentTestRuns.Remove(currentTest);
                _currentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));

                finalTest.TestInfo.SaveTestInfo(_locationsProvider);

                if (_reporterSettings.RealTimeGeneration)
                {
                    GenerateReport(DateTime.Now);
                }
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
