using System;
using System.Net;
using System.Net.Http;
using Ghpr.CouchDb.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ghpr.CouchDb
{
    public class CouchDbClient : IDisposable
    {
        private readonly HttpClient _client;
        private static StringContent EmptyStringContent => new StringContent("");
        private const string GhprDatabaseName = "ghpr";
        
        public CouchDbClient(CouchDbSettings couchDbSettings)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(couchDbSettings.Endpoint)
            };
        }

        private static StringContent GetContent<T>(DatabaseEntity<T> entity)
        {
            return new StringContent(JsonConvert.SerializeObject(entity));
        }

        public void SaveReportSettings(DatabaseEntity<ReportSettings> reportSettingsEntity)
        {
            var settingsContent = GetContent(reportSettingsEntity);
            var postResult = _client.PutAsync($"/{GhprDatabaseName}/{reportSettingsEntity.Id}?new_edits=false", settingsContent).GetAwaiter().GetResult();
            var postResString = postResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jResult = JObject.Parse(postResString);
            if (postResult.StatusCode == HttpStatusCode.Created && (bool)jResult.SelectToken("ok"))
            {
                Console.WriteLine($"Report settings {JsonConvert.SerializeObject(reportSettingsEntity.Data, Formatting.Indented)} " +
                                  $"were saved successfully, result: {postResString}");
            }
            else
            {
                Console.WriteLine($"Report settings were not saved correctly: {postResString}");
            }
        }

        public void SaveTestRun(DatabaseEntity<TestRun> testRunEntity)
        {
            var testRunContent = GetContent(testRunEntity);
            var postResult = _client.PutAsync($"/{GhprDatabaseName}/{testRunEntity.Id}?new_edits=false", testRunContent).GetAwaiter().GetResult();
            var postResString = postResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jResult = JObject.Parse(postResString);
            if (postResult.StatusCode == HttpStatusCode.Created && (bool)jResult.SelectToken("ok"))
            {
                Console.WriteLine($"Test run {JsonConvert.SerializeObject(testRunEntity.Data.TestInfo, Formatting.Indented)} " +
                                      $"was created successfully, result: {postResString}");
            }
            else
            {
                Console.WriteLine($"Test run was not saved correctly: {postResString}");
            }
        }
        
        public void SaveRun(DatabaseEntity<Run> runEntity)
        {
            var runContent = GetContent(runEntity);
            var postResult = _client.PutAsync($"/{GhprDatabaseName}/{runEntity.Id}", runContent).GetAwaiter().GetResult();
            var postResString = postResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jResult = JObject.Parse(postResString);
            if (postResult.StatusCode == HttpStatusCode.Created && (bool)jResult.SelectToken("ok"))
            {
                Console.WriteLine($"Run {JsonConvert.SerializeObject(runEntity.Data.RunInfo, Formatting.Indented)} " +
                                      $"was saved successfully, result: {postResString}");
            }
            else
            {
                Console.WriteLine($"Run was not saved correctly: {postResString}");
            }
        }

        public void ValidateConnection()
        {
            var resultStr = _client.GetStringAsync("/").GetAwaiter().GetResult();
            var r = JObject.Parse(resultStr);
            var couchDb = (string)r.SelectToken("couchdb");
            var version = (string)r.SelectToken("version");
            var vendorName = (string)r.SelectToken("vendor").SelectToken("name");
            Console.WriteLine($"{couchDb}. CouchDB version is {version}. Vendor: {vendorName}");
            if (couchDb == null || version == null || vendorName == null)
            {
                throw new Exception($"Error while connecting to the database server: {resultStr}");
            }
        }

        public void CreateDb()
        {
            var postResult = _client.PutAsync($"/{GhprDatabaseName}", EmptyStringContent).GetAwaiter().GetResult();
            var resultContentString = postResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jResult = JObject.Parse(resultContentString);
            if (postResult.StatusCode == HttpStatusCode.Created)
            {
                var ok = (bool)jResult.SelectToken("ok");
                if (ok)
                {
                    Console.WriteLine($"Database {GhprDatabaseName} was created successfully, result: {resultContentString}");
                }
                else
                {
                    throw new Exception($"Database was not created correctly: {resultContentString}");
                }
            }
            else if (postResult.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                var fileExists = (string)jResult.SelectToken("error");
                if (fileExists != null)
                {
                    Console.WriteLine($"Database {GhprDatabaseName} already exists.");
                }
                else
                {
                    throw new Exception($"Unexpected error while creating database: {resultContentString}");
                }
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}