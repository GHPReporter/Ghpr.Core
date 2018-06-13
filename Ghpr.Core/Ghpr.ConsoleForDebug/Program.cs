using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Factories;

namespace Ghpr.ConsoleForDebug
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reporter = ReporterFactory.Build(new DummyTestDataProvider());
            
            var reportSettings = new ReportSettingsDto(5, 7);
            reporter.DataService.SaveReportSettings(reportSettings);
            reporter.DataService.SaveReportSettings(reportSettings);
            reporter.DataService.SaveReportSettings(reportSettings);

            var run = new RunDto
            {
                RunInfo = new ItemInfoDto
                {
                    Start = DateTime.Now.AddMinutes(-2),
                    Finish = DateTime.Now,
                    Guid = Guid.NewGuid()
                },
                RunSummary = new RunSummaryDto(),
                Name = "Awesome run",
                Sprint = "Sprint 1",
                TestsInfo = new List<ItemInfoDto>()
            };
            reporter.DataService.SaveRun(run);
            reporter.DataService.SaveRun(run);

            var testGuid = Guid.NewGuid();
            var screen = new TestScreenshotDto
            {
                Date = DateTime.Now,
                Data = new byte[]{1, 2, 0, 3},
                TestGuid = testGuid
            };
            var test = new TestRunDto(testGuid, "Test", "Test.FullName");
            var testInfo = new ItemInfoDto
            {
                Start = DateTime.Now.AddSeconds(-2),
                Finish = DateTime.Now.AddSeconds(2),
                Guid = testGuid
            };
            test.TestInfo = testInfo;
            reporter.DataService.SaveScreenshot(screen);
            reporter.DataService.SaveTestRun(test);

            Console.WriteLine("Done.");
        }
    }
}
