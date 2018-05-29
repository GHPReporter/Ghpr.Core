using System;
using Ghpr.Core.Common;
using Ghpr.Core.Factories;

namespace Ghpr.ConsoleForDebug
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reporter = ReporterFactory.Build(new DummyTestDataProvider());
            reporter.DataService.SaveReportSettings(new ReportSettingsDto(5, 7));
            Console.WriteLine("Done.");
        }
    }
}
