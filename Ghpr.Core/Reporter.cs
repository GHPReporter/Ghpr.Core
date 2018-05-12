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
            ReporterSettings = settings;
            if (ReporterSettings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(ReporterSettings.OutputPath),
                    "Reporter Output path must be specified. Please fix your .json settings file.");
            }
            ReportSettings = new ReportSettings(ReporterSettings.RunsToDisplay, ReporterSettings.TestsToDisplay);
            _action = new ActionHelper(ReporterSettings.OutputPath);
            _extractor = new ResourceExtractor(_action, ReporterSettings.OutputPath);
            _locationsProvider = new LocationsProvider(ReporterSettings.OutputPath);
            _dataProvider = new FileSystemDataProvider(ReporterSettings, _locationsProvider);
            TestRunStarted = false;
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
        private ResourceExtractor _extractor;
        private static ActionHelper _action;
        private ILocationsProvider _locationsProvider;
        private IDataProvider _dataProvider;

        public bool TestRunStarted { get; private set; }
        public IReportSettings ReportSettings { get; private set; }
        public IReporterSettings ReporterSettings { get; private set; }

        private void InitializeRun(DateTime startDateTime, string runGuid = "")
        {
            _action.Safe(() =>
            {
                var currentRunGuid = runGuid.Equals("") || runGuid.Equals("null") ? Guid.NewGuid() : Guid.Parse(runGuid);
                _currentRun = new Run(currentRunGuid)
                {
                    TestRunFiles = new List<string>(),
                    RunSummary = new RunSummary()
                };
                _currentTestRuns = new List<ITestRun>();
                _currentRun.Name = ReporterSettings.RunName;
                _currentRun.Sprint = ReporterSettings.Sprint;
                _extractor.ExtractReportBase();
                _currentRun.RunInfo.Start = startDateTime;
                ReportSettings.Save(_locationsProvider);
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            _action.Safe(() =>
            {
                _currentRun.RunInfo.Finish = finishDateTime;
                _currentRun.Save(_locationsProvider.RunsPath);
                var runInfo = new ItemInfo(_currentRun.RunInfo);
                runInfo.SaveCurrentRunInfo(_locationsProvider);
            });
        }

        public void RunStarted()
        {
            if (!TestRunStarted)
            {
                InitializeRun(DateTime.Now, ReporterSettings.RunGuid);
                TestRunStarted = true;
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
                var fileName = finishDateTime.GetTestName();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(testRun);

                testRun.TestInfo.FileName = fileName;
                testRun.RunGuid = _currentRun.RunInfo.Guid;
                testRun.TestInfo.Start = testRun.TestInfo.Start.Equals(default(DateTime))
                    ? finishDateTime
                    : testRun.TestInfo.Start;
                testRun.TestInfo.Finish = testRun.TestInfo.Finish.Equals(default(DateTime))
                    ? finishDateTime
                    : testRun.TestInfo.Finish;
                testRun.TestDuration = testRun.TestDuration.Equals(0.0)
                    ? (testRun.TestInfo.Finish - testRun.TestInfo.Start).TotalSeconds
                    : testRun.TestDuration;

                var testPath = _locationsProvider.GetTestPath(testGuid);
                testRun.Save(testPath, fileName);
                _currentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));

                testRun.TestInfo.SaveCurrentTestInfo(_locationsProvider);
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
                var fileName = finishDateTime.GetTestName();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                finalTest.TestInfo.FileName = fileName;
                finalTest.RunGuid = _currentRun.RunInfo.Guid;
                finalTest.TestInfo.Start = finalTest.TestInfo.Start.Equals(default(DateTime))
                    ? finishDateTime
                    : finalTest.TestInfo.Start;
                finalTest.TestInfo.Finish = finalTest.TestInfo.Finish.Equals(default(DateTime))
                    ? finishDateTime
                    : finalTest.TestInfo.Finish;
                finalTest.TestDuration = finalTest.TestDuration.Equals(0.0)
                    ? (finalTest.TestInfo.Finish - finalTest.TestInfo.Start).TotalSeconds
                    : finalTest.TestDuration;

                var testPath = _locationsProvider.GetTestPath(testGuid);

                finalTest.Save(testPath, fileName);
                _currentTestRuns.Remove(currentTest);
                _currentRun.TestRunFiles.Add(_locationsProvider.GetRelativeTestRunPath(testGuid, fileName));

                finalTest.TestInfo.SaveCurrentTestInfo(_locationsProvider);

                if (ReporterSettings.RealTimeGeneration)
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

            InitializeRun(start, ReporterSettings.RunGuid);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun);
            }
            GenerateReport(finish);
        }
    }
}
