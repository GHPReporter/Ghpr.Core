using System;
using System.Collections.Generic;
using System.IO;
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
        private IRun _currentRun;
        private List<ITestRun> _currentRunningTests;

        private static readonly ResourceExtractor Extractor = new ResourceExtractor(OutputPath);
        
        public static string OutputPath => Properties.Settings.Default.OutputPath;
        public static bool TakeScreenshotAfterFail => Properties.Settings.Default.TakeScreenshotAfterFail;
        public static string Sprint => Properties.Settings.Default.Sprint;
        public static string RunName => Properties.Settings.Default.RunName;
        public static bool RealTime => Properties.Settings.Default.RealTime;

        public const string TestsFolder = "tests";
        public const string RunsFolder = "runs";

        private void CleanUp()
        {
            _currentRun = new Run(Guid.NewGuid())
            {
                TestRunFiles = new List<string>(),
                RunSummary = new RunSummary()
            };
            _currentRunningTests = new List<ITestRun>();
        }
        
        public void RunStarted()
        {
            try
            {
                CleanUp();
                _currentRun.Name = RunName;
                _currentRun.Sprint = Sprint;
                Extractor.ExtractReportBase();
                _currentRun.RunInfo.Start = DateTime.Now;
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception in RunStarted");
            }
        }

        public void RunFinished()
        {
            try
            {
                _currentRun.RunInfo.Finish = DateTime.Now;
                var runsPath = Path.Combine(OutputPath, RunsFolder);
                _currentRun.Save(runsPath);
                RunsHelper.SaveCurrentRunInfo(runsPath, _currentRun.RunInfo);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception in RunFinished");
            }
        }
        
        public void TestStarted(ITestRun testRun)
        {
            try
            {
                _currentRunningTests.Add(testRun);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception in TestStarted");
            }
        }

        public void TestFinished(ITestRun testRun)
        {
            _currentRun.RunSummary.Total++;
            try
            {
                var finishDateTime = DateTime.Now;
                var currentTest = _currentRunningTests.GetTest(testRun);
                var finalTest = testRun.Update(currentTest);
                var testGuid = finalTest.TestInfo.Guid.ToString();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                var testsPath = Path.Combine(OutputPath, TestsFolder);
                var testPath = Path.Combine(testsPath, testGuid);
                var fileName = finishDateTime.GetTestName();
                finalTest.TestInfo.FileName = fileName;
                finalTest.RunGuid = _currentRun.RunInfo.Guid;
                if (finalTest.TestInfo.Start.Equals(default(DateTime)))
                {
                    finalTest.TestInfo.Start = finishDateTime;
                }
                if (finalTest.TestInfo.Finish.Equals(default(DateTime)))
                {
                    finalTest.TestInfo.Finish = finishDateTime;
                }
                finalTest
                    .TakeScreenshot(testPath, TakeScreenshotAfterFail)
                    .Save(testPath, fileName);
                _currentRunningTests.Remove(currentTest);
                _currentRun.TestRunFiles.Add($"{testGuid}\\{fileName}");
                Extractor.ExtractTestPage(testsPath);
                TestRunsHelper.SaveCurrentTestInfo(testPath, finalTest.TestInfo);

                if (RealTime)
                {
                    RunFinished();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex, $"Exception in TestFinished {testRun.FullName}");
            }
        }
    }
}
