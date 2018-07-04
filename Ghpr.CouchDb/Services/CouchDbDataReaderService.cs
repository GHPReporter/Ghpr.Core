using System;
using System.Collections.Generic;
using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.CouchDb.Services
{
    public class CouchDbDataReaderService : IDataReaderService
    {
        public void Initialize(ReporterSettings settings, ILogger logger)
        {
            var couchDbSettings = "Ghpr.CouchDb.Settings.json".LoadSettingsAs<CouchDbSettings>();
            Database = new CouchDbDatabase(couchDbSettings, logger);
            Database.CreateDb();
            Database.ValidateConnection();
        }

        public CouchDbDatabase Database { get; private set; }

        //TODO: implement later!

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            throw new NotImplementedException();
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public List<TestRunDto> GetTestRuns(Guid testGuid)
        {
            throw new NotImplementedException();
        }

        public List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public List<TestOutputDto> GetTestOutput(ItemInfoDto testInfo)
        {
            throw new NotImplementedException();
        }

        public RunDto GetRun(Guid runGuid)
        {
            throw new NotImplementedException();
        }

        public List<RunDto> GetRuns()
        {
            throw new NotImplementedException();
        }

        public List<TestRunDto> GetTestRunsFromRun(Guid runGuid)
        {
            throw new NotImplementedException();
        }
    }
}