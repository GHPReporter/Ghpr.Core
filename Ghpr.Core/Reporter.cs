using System;
using System.Collections.Generic;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core
{
    public class Reporter
    {
        private IRun _currentRun;
        private List<ITestRun> _currentTests;

        private static readonly ResourceExtractor Extractor = new ResourceExtractor();
        
        public static string OutputPath => Properties.Settings.Default.OutputPath;
        public static bool ContinuousGeneration => Properties.Settings.Default.ContinuousGeneration;
        public static bool TakeScreenshotAfterFail => Properties.Settings.Default.TakeScreenshotAfterFail;
        public const string SrcFolder = "src";
        public const string TestsFolder = "Tests";
        public const string RunsFolder = "Runs";

        private void CleanUp()
        {
            _currentRun = new Run(Guid.NewGuid())
            {
                TestRunFiles = new List<string>(),
                RunSummary = new RunSummary()
            };
            _currentTests = new List<ITestRun>();
        }
        
        public void RunStarted()
        {
            try
            {
                CleanUp();
                Extractor.Extract(Resource.TestRunsPage, OutputPath);
                Extractor.Extract(Resource.All, Path.Combine(OutputPath, SrcFolder));
                _currentRun.Start = DateTime.Now;
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
                Extractor.Extract(Resource.TestRunPage, Path.Combine(OutputPath, RunsFolder));
                _currentRun.Finish = DateTime.Now;
                _currentRun.Save(Path.Combine(OutputPath, RunsFolder));
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
                _currentTests.Add(testRun);
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
                var test = _currentTests.GetTest(testRun);
                var updatedTest = test.UpdateWith(testRun);
                var testGuid = updatedTest.Guid.ToString();

                _currentRun.RunSummary = _currentRun.RunSummary.Update(updatedTest);

                var path = Path.Combine(OutputPath, TestsFolder, testGuid);
                var fileName = finishDateTime.GetTestName();
                updatedTest
                    .TakeScreenshot(path, TakeScreenshotAfterFail)
                    .Save(path, fileName);
                _currentTests = _currentTests.RemoveTest(test);
                _currentRun.TestRunFiles.Add($"{testGuid}\\{fileName}");
                Extractor.Extract(Resource.TestPage, path);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, $"Exception in TestFinished {testRun.FullName}");
            }
        }
    }
}
