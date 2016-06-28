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
        public static bool ContinuousGeneration => Properties.Settings.Default.ContinuousGeneration;
        public static bool TakeScreenshotAfterFail => Properties.Settings.Default.TakeScreenshotAfterFail;
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
                CleanUp();
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

                var path = Path.Combine(OutputPath, TestsFolder, testGuid);
                var fileName = finishDateTime.GetTestName();
                finalTest.RunGuid = _currentRun.RunInfo.Guid;
                finalTest
                    .TakeScreenshot(path, TakeScreenshotAfterFail)
                    .Save(path, fileName);
                _currentRunningTests.Remove(currentTest);
                _currentRun.TestRunFiles.Add($"{testGuid}\\{fileName}");
                Extractor.ExtractTestPage(path);
                TestRunsHelper.SaveCurrentTestInfo(path, finalTest.TestInfo);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, $"Exception in TestFinished {testRun.FullName}");
            }
        }
    }
}
