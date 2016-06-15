using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;
using Ghpr.Core.Extensions;
using Ghpr.Core.HtmlPages;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core
{
    public class Reporter
    {
        private static IRun _currentRun;
        private static List<ITestRun> _currentTests;

        public Reporter()
        {
            _currentRun = null;
            _currentTests = null;
        }

        public static string OutputFolder => Properties.Settings.Default.outputPath;
        public const string SrcFolder = "src";
        public const string TestsFolder = "Tests";
        public const string RunsFolder = "Runs";

        private static void ExtractReportBase()
        {
            var re = new ResourceExtractor(Path.Combine(OutputFolder, SrcFolder));

            var repornMainPage = new ReportMainPage(SrcFolder);
            repornMainPage.SavePage(OutputFolder, "index.html");

            re.Extract(Resource.All);
        }
        
        public void RunStarted()
        {
            _currentTests = new List<ITestRun>();
            _currentRun = new Run(Guid.NewGuid())
            {
                TestRunFiles = new List<string>()
            };
            ExtractReportBase();
        }

        public void RunFinished()
        {
            _currentRun.Save(Path.Combine(OutputFolder, RunsFolder));
        }

        public void TestStarted(string testGuid)
        {
            var testRun = new TestRun(testGuid);
            _currentTests.Add(testRun);
        }

        public void TestFinished(string testGuid)
        {
            var finishDateTime = DateTime.Now;
            var path = Path.Combine(OutputFolder, TestsFolder, testGuid);
            var name = finishDateTime.GetTestName();
            _currentTests.First(t => t.Guid.Equals(Guid.Parse(testGuid)))
                .SetFinishDateTime(finishDateTime)
                .Save(path, name);
            _currentRun.TestRunFiles.Add($"{testGuid}\\{name}");
        }
    }
}
