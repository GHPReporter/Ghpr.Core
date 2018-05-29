using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb
{
    public class CouchDbDatabase
    {
        private readonly CouchDbSettings _couchDbSettings;

        public CouchDbDatabase(CouchDbSettings couchDbSettings)
        {
            _couchDbSettings = couchDbSettings;
        }

        public void CreateDb()
        {
            using (var client = new CouchDbClient(_couchDbSettings))
            {
                client.CreateDb();
            }
        }

        public void SaveTestRun(DatabaseEntity<TestRun> testRunEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings))
            {
                client.SaveTestRun(testRunEntity);
            }
        }

        public void SaveReportSettings(DatabaseEntity<ReportSettings> reportSettingsEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings))
            {
                client.SaveReportSettings(reportSettingsEntity);
            }
        }

        public void SaveRun(DatabaseEntity<Run> runEntity)
        {
            using (var client = new CouchDbClient(_couchDbSettings))
            {
                client.SaveRun(runEntity);
            }
        }

        public void ValidateConnection()
        {
            using (var client = new CouchDbClient(_couchDbSettings))
            {
                client.ValidateConnection();
            }
        }
    }
}