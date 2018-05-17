using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IDataService
    {
        ReporterSettingsDto ReporterSettings { get; }

        void SaveReportSettings(ReportSettingsDto reportSettings);
        void SaveTestRun(TestRunDto testRun);
        void SaveRun(RunDto run);
    }
}