using System;
using System.Net;
using System.Net.Http;
using Ghpr.CouchDb.Entities;
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

        public void SavetestRun(TestRun testRun)
        {

        }

        public bool TestConnection()
        {
            var resultStr = _client.GetStringAsync("/").GetAwaiter().GetResult();
            var r = JObject.Parse(resultStr);
            var couchDb = (string)r.SelectToken("couchdb");
            var version = (string)r.SelectToken("version");
            var vendorName = (string)r.SelectToken("vendor").SelectToken("name");
            //Console.WriteLine($"{couchDb}. CouchDB version is {version}. Vendor: {vendorName}");
            return couchDb != null && version != null && vendorName != null;
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
                    //Console.WriteLine($"Database {GhprDatabaseName} was created successfully, result: {resultContentString}");
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
                    //Console.WriteLine($"Database {GhprDatabaseName} already exists");
                }
                else
                {
                    throw new Exception($"Unexpected error while creating database: {resultContentString}");
                }
            }
            Console.WriteLine(resultContentString);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}