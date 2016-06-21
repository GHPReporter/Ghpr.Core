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
        private static IRun _currentRun;
        private static List<ITestRun> _currentTests;
        private static readonly ResourceExtractor Extractor = new ResourceExtractor();
        
        public static string OutputPath => Properties.Settings.Default.OutputPath;
        public static bool ContinuousGeneration => Properties.Settings.Default.ContinuousGeneration;
        public static bool TakeScreenshotAfterFail => Properties.Settings.Default.TakeScreenshotAfterFail;
        public const string SrcFolder = "src";
        public const string TestsFolder = "Tests";
        public const string RunsFolder = "Runs";

        private static void CleanUp()
        {
            _currentRun = new Run(Guid.NewGuid())
            {
                TestRunFiles = new List<string>()
            };
        }
        
        public void RunStarted()
        {
            try
            {
                CleanUp();
                Extractor.Extract(Resource.TestRunsPage, OutputPath);
                Extractor.Extract(Resource.All, Path.Combine(OutputPath, SrcFolder));
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
            try
            {
                var testGuid = testRun.Guid.ToString();
                var finishDateTime = DateTime.Now;
                var path = Path.Combine(OutputPath, TestsFolder, testGuid);
                var fileName = finishDateTime.GetTestName();
                _currentTests.GetTest(testRun)
                    .SetFinishDateTime(finishDateTime)
                    .UpdateWith(testRun)
                    .TakeScreenshot(path, TakeScreenshotAfterFail)
                    .Save(path, fileName);
                _currentTests = _currentTests.RemoveTest(testRun);
                _currentRun.TestRunFiles.Add($"{testGuid}\\{fileName}");
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception in TestFinished");
            }
        }
    }
}
