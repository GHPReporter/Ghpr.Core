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

            Console.WriteLine("Done.");
        }
    }
}
