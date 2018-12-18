using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;

namespace Ghpr.CouchDb.Services
{
    public class CouchDbDataReaderService : IDataReaderService
    {
        public IDataReaderService GetDataReader()
        {
            return this;
        }

        public void InitializeDataReader(ReporterSettings settings, ILogger logger)
        {
            var couchDbSettings = "Ghpr.CouchDb.Settings.json".LoadSettingsAs<CouchDbSettings>();
            Database = new CouchDbDatabase(couchDbSettings, logger);
            Database.CreateDb();
            Database.ValidateConnection();
        }
        
        public CouchDbDatabase Database { get; private set; }

        //TODO: implement later!

        public ReportSettingsDto GetReportSettings()
        {
            throw new NotImplementedException();
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            throw new NotImplementedException();
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public List<ItemInfoDto> GetTestInfos(Guid testGuid)
        {
            throw new NotImplementedException();
        }

        public List<TestScreenshotDto> GetTestScreenshots(TestRunDto testInfo)
        {
            throw new NotImplementedException();
        }

        public TestOutputDto GetTestOutput(TestRunDto testInfo)
        {
            throw new NotImplementedException();
        }

        public RunDto GetRun(Guid runGuid)
        {
            throw new NotImplementedException();
        }

        public List<ItemInfoDto> GetRunInfos()
        {
            throw new NotImplementedException();
        }

        public List<TestRunDto> GetTestRunsFromRun(RunDto runGuid)
        {
            throw new NotImplementedException();
        }
    }
}