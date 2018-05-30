using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Ghpr.CouchDb.Entities;
using Ghpr.CouchDb.Extensions;
using Ghpr.CouchDb.Processors;
using Ghpr.CouchDb.Utils;

namespace Ghpr.CouchDb
{
    public class CouchDbClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly StringContentBuilder _contentBuilder;
        private readonly HttpResponseMessageProcessor _messageProcessor;
        private readonly string _ghprDatabaseName;
        
        public CouchDbClient(CouchDbSettings couchDbSettings)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(couchDbSettings.Endpoint)
            };
            _contentBuilder = new StringContentBuilder();
            _messageProcessor = new HttpResponseMessageProcessor();
            _ghprDatabaseName = couchDbSettings.Database;
        }

        public void SaveReportSettings(DatabaseEntity<ReportSettings> reportSettingsEntity)
        {
            var searchContent = _contentBuilder.FindReportSettingsContent();
            var postRunsQueryResult = _client.Find(_ghprDatabaseName, searchContent);
            var existingRunRevision = postRunsQueryResult.ContentAsJObject().SelectToken("docs")
                .First?.ToObject<DatabaseEntity<ReportSettings>>()?.Rev ?? "";
            reportSettingsEntity.Rev = existingRunRevision.Equals("") ? reportSettingsEntity.Rev : existingRunRevision;
            var revParam = existingRunRevision.Equals("") ? "" : $"?rev={existingRunRevision}";

            var settingsContent = _contentBuilder.GetContent(reportSettingsEntity);
            var postResult = _client.Put($"/{_ghprDatabaseName}/{reportSettingsEntity.Id}{revParam}", settingsContent);
            _messageProcessor.ProcessReportSettingsSavedMessage(postResult, reportSettingsEntity.Data);
        }

        public void SaveScreenshot(DatabaseEntity<TestScreenshot> screenshotEntity)
        {
            var settingsContent = _contentBuilder.GetContent(screenshotEntity);
            var postResult = _client.Put($"/{_ghprDatabaseName}/{screenshotEntity.Id}?new_edits?=false", settingsContent);
            _messageProcessor.ProcessScreenshotSavedMessage(postResult, screenshotEntity.Data.TestGuid.ToString(), screenshotEntity.Data.Date);
        }

        public void SaveTestRun(DatabaseEntity<TestRun> testRunEntity)
        {
            var screenshotSearchContent =
                _contentBuilder.FindTestScreenshotsByTestGuid(testRunEntity.Data.TestInfo.Guid.ToString(), 
                    testRunEntity.Data.TestInfo.Start, testRunEntity.Data.TestInfo.Finish);
            var findResult = _client.Find(_ghprDatabaseName, screenshotSearchContent);
            var screenshots = findResult.ContentAsJObject().SelectToken("docs").ToObject<List<DatabaseEntity<TestScreenshot>>>();
            testRunEntity.Data.Screenshots.AddRange(screenshots.Select(screenshotEntity => new TestScreenshotInfo
            {
                Date = screenshotEntity.Data.Date,
                Id = screenshotEntity.Id,
                Revision = screenshotEntity.Rev
            }));
            var testRunContent = _contentBuilder.GetContent(testRunEntity);
            var response = _client.Put($"/{_ghprDatabaseName}/{testRunEntity.Id}?new_edits=false", testRunContent);
            _messageProcessor.ProcessTestRunSavedMessage(response, testRunEntity.Data.TestInfo);
        }
        
        public void SaveRun(DatabaseEntity<Run> runEntity)
        {
            var searchContent = _contentBuilder.FindRunsContent(runEntity.Data.RunInfo.Guid.ToString());
            var postRunsQueryResult = _client.Find(_ghprDatabaseName, searchContent);
            var existingRunRevision = postRunsQueryResult.ContentAsJObject().SelectToken("docs")
                .First?.ToObject<DatabaseEntity<Run>>()?.Rev ?? "";
            runEntity.Rev = existingRunRevision.Equals("") ? runEntity.Rev : existingRunRevision;
            var revParam = existingRunRevision.Equals("") ? "" : $"?rev={existingRunRevision}";
            var runContent = _contentBuilder.GetContent(runEntity);
            var postResult = _client.Put($"/{_ghprDatabaseName}/{runEntity.Id}{revParam}", runContent);
            _messageProcessor.ProcessRunSavedMessage(postResult, runEntity.Data.RunInfo);
        }
        
        public void ValidateConnection()
        {
            var resultStr = _client.GetString("/");
            _messageProcessor.ProcessValidateConnectionMessage(resultStr);
        }

        public void CreateDb()
        {
            var putResult = _client.Put($"/{_ghprDatabaseName}", StringContentBuilder.Empty);
            _messageProcessor.ProcessCreateDbMessage(putResult, _ghprDatabaseName);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}