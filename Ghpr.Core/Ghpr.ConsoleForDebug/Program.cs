using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Factories;

namespace Ghpr.ConsoleForDebug
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reporter = ReporterFactory.Build(new DummyTestDataProvider());
            
            ResourceExtractor.ExtractReportBase(reporter.ReporterSettings.OutputPath);

            reporter.Logger.Info("STARTED");

            var reportSettings = new ReportSettingsDto(5, 7);
            reporter.DataWriterService.SaveReportSettings(reportSettings);
            reporter.DataWriterService.SaveReportSettings(reportSettings);
            reporter.DataWriterService.SaveReportSettings(reportSettings);

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
            reporter.DataWriterService.SaveRun(run);
            reporter.DataWriterService.SaveRun(run);

            reporter.Logger.Info("RUN SAVED");

            var testGuid = Guid.NewGuid();
            var screen = new TestScreenshotDto
            {
                TestScreenshotInfo = new SimpleItemInfoDto
                {
                    Date = DateTime.Now,
                    ItemName = "Screenshot"
                },
                Base64Data = "ASDJasdkajasdfas==",
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
            reporter.DataWriterService.SaveScreenshot(screen);
            reporter.DataWriterService.SaveTestRun(test, new TestOutputDto
            {
                TestOutputInfo = new SimpleItemInfoDto
                {
                    Date = DateTime.Now,
                    ItemName = "Some output"
                },
                Output = "output",
                SuiteOutput = "suite output"
            });

            reporter.Logger.Info("DONE");
            reporter.TearDown();
        }
    }
}
