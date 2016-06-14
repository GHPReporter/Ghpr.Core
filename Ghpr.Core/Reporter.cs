using System;
using System.Collections.Generic;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Enums;
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
            _currentRun = new Run(Guid.NewGuid());
            _currentTests = new List<ITestRun>();
        }

        public static string OutputPath => Properties.Settings.Default.outputPath;
        public const string Src = "src";

        private void ExtractReportBase()
        {
            var re = new ResourceExtractor(Path.Combine(OutputPath, Src));

            var repornMainPage = new ReportMainPage(Src);
            repornMainPage.SavePage(OutputPath, "index.html");

            re.Extract(Resource.All);
        }
        
        public void RunStarted()
        {
            ExtractReportBase();
        }

        public void RunFinished()
        {

        }

        public void TestStarted(ITestRun testRun)
        {
            
        }

        public void TestFinished(ITestRun testRun)
        {
        }
    }
}
