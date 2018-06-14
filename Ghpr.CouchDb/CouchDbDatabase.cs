using Ghpr.Core.Interfaces;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb
{
    public class CouchDbDatabase
    {
        private readonly CouchDbSettings _couchDbSettings;
        private readonly ILogger _logger;

        public CouchDbDatabase(CouchDbSettings couchDbSettings, ILogger logger)
        {
            _couchDbSettings = couchDbSettings;
            _logger = logger;
        }

        public void CreateDb()
        {
            using (var client = new CouchDbClient(_couchDbSettings, _logger))
            {
                client.CreateDb();
            }
        }

        public void SaveTestRun(DatabaseEntity<TestRun> testRunEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings, _logger))
            {
                client.SaveTestRun(testRunEntity);
            }
        }

        public void SaveScreenshot(DatabaseEntity<TestScreenshot> testScreenshotEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings, _logger))
            {
                client.SaveScreenshot(testScreenshotEntity);
            }
        }

        public void SaveReportSettings(DatabaseEntity<ReportSettings> reportSettingsEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings, _logger))
            {
                client.SaveReportSettings(reportSettingsEntity);
            }
        }

        public void SaveRun(DatabaseEntity<Run> runEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings, _logger))
            {
                client.SaveRun(runEntity);
            }
        }

        public void ValidateConnection()
        {
            using (var client = new CouchDbClient(_couchDbSettings, _logger))
            {
                client.ValidateConnection();
            }
        }
    }
}