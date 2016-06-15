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

        public static string OutputPath => Properties.Settings.Default.outputPath;
        public const string Src = "src";
        public const string Tests = "Tests";
        public const string Runs = "Runs";

        private static void ExtractReportBase()
        {
            var re = new ResourceExtractor(Path.Combine(OutputPath, Src));

            var repornMainPage = new ReportMainPage(Src);
            repornMainPage.SavePage(OutputPath, "index.html");

            re.Extract(Resource.All);
        }
        
        public void RunStarted()
        {
            _currentTests = new List<ITestRun>();
            _currentRun = new Run(Guid.NewGuid());
            ExtractReportBase();
        }

        public void RunFinished()
        {
            _currentRun.Save(Path.Combine(OutputPath, "Runs"));
        }

        public void TestStarted(string testGuid)
        {
            var testRun = new TestRun(testGuid);
            _currentTests.Add(testRun);
        }

        public void TestFinished(string testGuid)
        {
            var finishDateTime = DateTime.Now;
            _currentTests.First(t => t.Guid.Equals(Guid.Parse(testGuid)))
                .SetFinishDateTime(finishDateTime)
                .Save(Path.Combine(OutputPath, "Tests", testGuid));

            _currentRun.TestRunFiles.Add(Path.Combine("Tests"));
        }
    }
}
