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
            var couchDbSettings = "Ghpr.CouchDb.Settings.json".Load<CouchDbSettings>();
            Client = new CouchDbClient(couchDbSettings);
            Client.CreateDb();
            Client.ValidateConnection();
        }

        public ReporterSettings ReporterSettings { get; private set; }
        public CouchDbClient Client { get; private set; }

        public void SaveRun(RunDto runDto)
        {
            var run = runDto.Map();

        }
        
        public void SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            var screenshot = testScreenshot.Map();

        }

        public void SaveReportSettings(ReportSettingsDto reportSettingsDto)
        {
            var reportSettings = reportSettingsDto.Map();

        }

        public void SaveTestRun(TestRunDto testRunDto)
        {
            var testRun = testRunDto.Map();
        }
    }
}