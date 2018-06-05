using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Ghpr.CouchDb.Mappers;

namespace Ghpr.CouchDb.Services
{
    public class CouchDbDataService : IDataService
    {
        public void Initialize(ReporterSettings settings)
        {
            ReporterSettings = settings;
            var couchDbSettings = "Ghpr.CouchDb.Settings.json".LoadAs<CouchDbSettings>();
            Database = new CouchDbDatabase(couchDbSettings);
            Database.CreateDb();
            Database.ValidateConnection();
        }

        public ReporterSettings ReporterSettings { get; private set; }
        public CouchDbDatabase Database { get; private set; }
        
        public void SaveRun(RunDto runDto)
        {
            var runEntity = runDto.Map();
            Database.SaveRun(runEntity);
        }
        
        public void SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            var screenshotEntity = testScreenshot.Map();
            Database.SaveScreenshot(screenshotEntity);
        }

        public void SaveReportSettings(ReportSettingsDto reportSettingsDto)
        {
            var reportSettingsEntity = reportSettingsDto.Map();
            Database.SaveReportSettings(reportSettingsEntity);
        }

        public void SaveTestRun(TestRunDto testRunDto)
        {
            var testRunEntity = testRunDto.Map();
            Database.SaveTestRun(testRunEntity);
        }
    }
}