using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Ghpr.CouchDb.Mappers;

namespace Ghpr.CouchDb.Services
{
    public class CouchDbDataService : IDataService
    {
        public void Initialize(ReporterSettings settings, ILogger logger)
        {
            var couchDbSettings = "Ghpr.CouchDb.Settings.json".LoadSettingsAs<CouchDbSettings>();
            Database = new CouchDbDatabase(couchDbSettings, logger);
            Database.CreateDb();
            Database.ValidateConnection();
        }

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

        public void SaveTestRun(TestRunDto testRunDto, TestOutputDto testOutputDto)
        {
            //TODO: SAVE testOutputDto correctly!
            var testRunEntity = testRunDto.Map();
            Database.SaveTestRun(testRunEntity);
        }
    }
}