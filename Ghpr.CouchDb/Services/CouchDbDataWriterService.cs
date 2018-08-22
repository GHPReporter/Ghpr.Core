using System;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using Ghpr.CouchDb.Mappers;

namespace Ghpr.CouchDb.Services
{
    public class CouchDbDataWriterService : IDataWriterService
    {
        public void InitializeDataWriter(ReporterSettings settings, ILogger logger)
        {
            var couchDbSettings = "Ghpr.CouchDb.Settings.json".LoadSettingsAs<CouchDbSettings>();
            Database = new CouchDbDatabase(couchDbSettings, logger);
            Database.CreateDb();
            Database.ValidateConnection();
        }

        public CouchDbDatabase Database { get; private set; }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            //TODO: implement later!
            throw new System.NotImplementedException();
        }

        public ItemInfoDto SaveRun(RunDto runDto)
        {
            var runEntity = runDto.Map();
            Database.SaveRun(runEntity);
            return runEntity.Data.RunInfo.ToDto();
        }
        
        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            var screenshotEntity = testScreenshot.Map();
            Database.SaveScreenshot(screenshotEntity);
            return screenshotEntity.Data.TestScreenshotInfo.ToDto();
        }

        public void DeleteRun(Guid runGuid)
        {
            //TODO: implement later
            throw new NotImplementedException();
        }

        public void SaveReportSettings(ReportSettingsDto reportSettingsDto)
        {
            var reportSettingsEntity = reportSettingsDto.Map();
            Database.SaveReportSettings(reportSettingsEntity);
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRunDto, TestOutputDto testOutputDto)
        {
            //TODO: SAVE testOutputDto correctly!
            var testRunEntity = testRunDto.Map();
            Database.SaveTestRun(testRunEntity);
            return testRunEntity.Data.TestInfo.ToDto();
        }
    }
}