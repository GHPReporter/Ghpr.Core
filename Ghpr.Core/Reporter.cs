using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Extensions;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core
{
    public class Reporter
    {
        public Reporter(IReporterSettings settings)
        {
            OutputPath = settings.OutputPath;
            TakeScreenshotAfterFail = settings.TakeScreenshotAfterFail;
            Sprint = settings.Sprint;
            RunName = settings.RunName;
            RunGuid = settings.RunGuid;
            RealTimeGeneration = settings.RealTimeGeneration;

            _actionHelper = new ActionHelper(OutputPath);
            _extractor = new ResourceExtractor(_actionHelper, OutputPath);
        }

        private IRun _currentRun;
        private ITestRun _currentTestRun;
        private List<ITestRun> _currentTestRuns;
        private Guid _currentRunGuid;

        private readonly object _lock = new object();

        private readonly ActionHelper _actionHelper;
        private readonly ResourceExtractor _extractor;

        public const string TestsFolderName = "tests";
        public const string RunsFolderName = "runs";
        public const string ImgFolderName = "img";

        public string OutputPath { get; }
        public bool TakeScreenshotAfterFail { get; }
        public string Sprint { get; }
        public string RunName { get; }
        public string RunGuid { get; }
        public bool RealTimeGeneration { get; }
        public string TestsPath => Path.Combine(OutputPath, TestsFolderName);
        public string RunsPath => Path.Combine(OutputPath, RunsFolderName);

        public void InitializeRun(DateTime startDateTime, string runGuid = "")
        {
            _actionHelper.SafeAction(() =>
            {
                _currentRunGuid = runGuid.Equals("") || runGuid.Equals("null") ? Guid.NewGuid() : Guid.Parse(runGuid);
                _currentRun = new Run(_currentRunGuid)
                {
                    TestRunFiles = new List<string>(),
                    RunSummary = new RunSummary()
                };
                _currentTestRuns = new List<ITestRun>();

                _currentRun.Name = RunName;
                _currentRun.Sprint = Sprint;
                _extractor.ExtractReportBase();
                _currentRun.RunInfo.Start = startDateTime;
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            lock (_lock)
            {
                _actionHelper.SafeAction(() =>
                {
                    _currentRun.RunInfo.Finish = finishDateTime;
                    _currentRun.Save(RunsPath);
                    RunsHelper.SaveCurrentRunInfo(RunsPath, _currentRun.RunInfo);
                });
            }
        }

        public void RunStarted()
        {
            InitializeRun(DateTime.Now, RunGuid);
        }

        public void RunFinished()
        {
            GenerateReport(DateTime.Now);
        }
        
        public void TestStarted(ITestRun testRun)
        {
            lock (_lock)
            {
                _actionHelper.SafeAction(() =>
                {
                    _currentTestRuns.Add(testRun);
                    _currentTestRun = testRun;
                });
            }
        }

        public void SaveScreenshot(Bitmap screen)
        {
            lock (_lock)
            {
                _actionHelper.SafeAction(() =>
                {
                    var testGuid = _currentTestRun.TestInfo.Guid.ToString();
                    var date = DateTime.Now;
                    var s = new TestScreenshot(date);
                    Taker.SaveScreenshot(GetScreenPath(testGuid), screen, date);
                    _currentTestRun.Screenshots.Add(s);
                    _currentTestRuns.First(
                        tr => tr.TestInfo.Guid.Equals(_currentTestRun.TestInfo.Guid))
                        .Screenshots.Add(s);
                });
            }
        }

        public string GetScreenPath(string testGuid)
        {
            lock (_lock)
            {
                return Path.Combine(TestsPath, testGuid, ImgFolderName);
            }
        }

        public void AddCompleteTestRun(ITestRun testRun)
        {
            lock (_lock)
            {
                _actionHelper.SafeAction(() =>
                {
                    _currentRun.RunSummary.Total++;

                    var finishDateTime = testRun.TestInfo.Finish;
                    var testGuid = testRun.TestInfo.Guid.ToString();
                    var testPath = Path.Combine(TestsPath, testGuid);
                    var fileName = finishDateTime.GetTestName();

                    _currentRun.RunSummary = _currentRun.RunSummary.Update(testRun);

                    testRun.TestInfo.FileName = fileName;
                    testRun.RunGuid = _currentRunGuid;
                    testRun.TestInfo.Start = testRun.TestInfo.Start.Equals(default(DateTime))
                        ? finishDateTime
                        : testRun.TestInfo.Start;
                    testRun.TestInfo.Finish = testRun.TestInfo.Finish.Equals(default(DateTime))
                        ? finishDateTime
                        : testRun.TestInfo.Finish;
                    testRun.TestDuration = testRun.TestDuration.Equals(0.0)
                        ? (testRun.TestInfo.Finish - testRun.TestInfo.Start).TotalSeconds
                        : testRun.TestDuration;
                    testRun.Save(testPath, fileName);
                    _currentRun.TestRunFiles.Add($"{testGuid}\\{fileName}");

                    TestRunsHelper.SaveCurrentTestInfo(testPath, testRun.TestInfo);

                });
            }
        }

        public void TestFinished(ITestRun testRun)
        {
            lock (_lock)
            {
                _actionHelper.SafeAction(() =>
                {
                    _currentRun.RunSummary.Total++;

                    var finishDateTime = DateTime.Now;
                    var currentTest = _currentTestRuns.GetTest(testRun);
                    var finalTest = testRun.Update(currentTest);
                    var testGuid = finalTest.TestInfo.Guid.ToString();
                    var testPath = Path.Combine(TestsPath, testGuid);
                    var fileName = finishDateTime.GetTestName();

                    _currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                    finalTest.TestInfo.FileName = fileName;
                    finalTest.RunGuid = _currentRunGuid;
                    finalTest.TestInfo.Start = finalTest.TestInfo.Start.Equals(default(DateTime))
                        ? finishDateTime
                        : finalTest.TestInfo.Start;
                    finalTest.TestInfo.Finish = finalTest.TestInfo.Finish.Equals(default(DateTime))
                        ? finishDateTime
                        : finalTest.TestInfo.Finish;
                    finalTest.TestDuration = finalTest.TestDuration.Equals(0.0)
                        ? (finalTest.TestInfo.Finish - finalTest.TestInfo.Start).TotalSeconds
                        : finalTest.TestDuration;
                    finalTest
                        .TakeScreenshot(Path.Combine(testPath, ImgFolderName), TakeScreenshotAfterFail)
                        .Save(testPath, fileName);
                    _currentTestRuns.Remove(currentTest);
                    _currentRun.TestRunFiles.Add($"{testGuid}\\{fileName}");

                    TestRunsHelper.SaveCurrentTestInfo(testPath, finalTest.TestInfo);

                    if (RealTimeGeneration)
                    {
                        GenerateReport(DateTime.Now);
                    }
                });
            }
        }

        public void GenerateFullReport(List<ITestRun> testRuns, string runGuid = "")
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }
            var runStart = testRuns.OrderBy(t => t.TestInfo.Start).First().TestInfo.Start;
            var runFinish = testRuns.OrderByDescending(t => t.TestInfo.Finish).First().TestInfo.Finish;
            GenerateFullReport(testRuns, runStart, runFinish, runGuid);
        }

        public void GenerateFullReport(List<ITestRun> testRuns, DateTime start, DateTime finish, string runGuid = "")
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }

            InitializeRun(start, runGuid);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun);
            }
            GenerateReport(finish);
        }
    }
}
