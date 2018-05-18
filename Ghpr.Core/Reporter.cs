using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;
using Ghpr.Core.Extensions;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Processors;
using Ghpr.Core.Providers;
using Ghpr.Core.Utils;

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
            _runRepository = new RunDtoRepository();
            _testRunDtosRepository = new TestRunDtosRepository();
            _testRunDtoProcessor = new TestRunDtoProcessor();
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

        private IRunDtoRepository _runRepository;
        private ITestRunDtosRepository _testRunDtosRepository;
        private ITestRunDtoProcessor _testRunDtoProcessor;
        private static ActionHelper _action;
        private IDataService _dataService;
        private bool _testRunStarted;
        private ReportSettingsDto _reportSettings;
        private ReporterSettings _reporterSettings;
        
        private void InitializeRun(DateTime startDateTime)
        {
            _action.Safe(() =>
            {
                _runRepository.OnRunStarted(_reporterSettings, startDateTime);
                _testRunDtosRepository.OnRunStarted();
                ResourceExtractor.ExtractReportBase(_reporterSettings.OutputPath);
                _dataService.SaveReportSettings(_reportSettings);
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            _action.Safe(() =>
            {
                _runRepository.OnRunFinished(finishDateTime);
                _dataService.SaveRun(_runRepository.CurrentRun);
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
                _testRunDtosRepository.AddNewTestRun(testRun);
            });
        }

        public void AddCompleteTestRun(TestRunDto testRun)
        {
            OnTestFinish(testRun);
        }

        public void TestFinished(TestRunDto testRun)
        {
            OnTestFinish(testRun);
            if (_reporterSettings.RealTimeGeneration)
            {
                GenerateReport(DateTime.Now);
            }
        }

        private void OnTestFinish(TestRunDto testDtoWhenFinished)
        {
            _action.Safe(() =>
            {
                _runRepository.OnTestFinished(testDtoWhenFinished);

                var testDtoWhenStarted = _testRunDtosRepository.ExtractCorrespondingTestRun(testDtoWhenFinished);
                var finalTest = _testRunDtoProcessor.Process(testDtoWhenStarted, testDtoWhenFinished, _runRepository.RunGuid);

                _dataService.SaveTestRun(finalTest);
            });
        }

        public void GenerateFullReport(List<TestRunDto> testRuns)
        {
            GenerateFullReport(testRuns, testRuns.GetRunStartDateTime(), testRuns.GetRunFinishDateTime());
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
