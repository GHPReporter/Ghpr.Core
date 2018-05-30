using System;
using System.Net;
using System.Net.Http;
using Ghpr.CouchDb.Entities;
using Ghpr.CouchDb.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ghpr.CouchDb.Processors
{
    public class HttpResponseMessageProcessor
    {
        public void ProcessScreenshotSavedMessage(HttpResponseMessage response, string testGuid, DateTime screenshotDateTime)
        {
            var postResString = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)postResString.SelectToken("ok"))
            {
                Console.WriteLine($"Screenshot for test {testGuid} with date time {screenshotDateTime} " +
                                  $"was saved successfully, result: {postResString}");
            }
            else
            {
                Console.WriteLine($"Screenshot was not saved correctly: {postResString}");
            }
        }

        public void ProcessReportSettingsSavedMessage(HttpResponseMessage response, ReportSettings reportSettings)
        {
            var postResString = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)postResString.SelectToken("ok"))
            {
                Console.WriteLine($"Report settings {JsonConvert.SerializeObject(reportSettings, Formatting.Indented)} " +
                                  $"were saved successfully, result: {postResString}");
            }
            else
            {
                Console.WriteLine($"Report settings were not saved correctly: {postResString}");
            }
        }

        public void ProcessTestRunSavedMessage(HttpResponseMessage response, ItemInfo itemInfo)
        {
            var jContent = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)jContent.SelectToken("ok"))
            {
                Console.WriteLine($"Test run {JsonConvert.SerializeObject(itemInfo, Formatting.Indented)} " +
                                  $"was created successfully, result: {jContent}");
            }
            else
            {
                Console.WriteLine($"Test run was not saved correctly: {jContent}");
            }
        }

        public void ProcessRunSavedMessage(HttpResponseMessage response, ItemInfo itemInfo)
        {
            var jContent = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)jContent.SelectToken("ok"))
            {
                Console.WriteLine($"Run {JsonConvert.SerializeObject(itemInfo, Formatting.Indented)} " +
                                  $"was saved successfully, result: {jContent}");
            }
            else
            {
                Console.WriteLine($"Run was not saved correctly: {jContent}");
            }
        }

        public void ProcessValidateConnectionMessage(string response)
        {
            var r = JObject.Parse(response);
            var couchDb = (string)r.SelectToken("couchdb");
            var version = (string)r.SelectToken("version");
            var vendorName = (string)r.SelectToken("vendor").SelectToken("name");
            //Console.WriteLine($"{couchDb}. CouchDB version is {version}. Vendor: {vendorName}");
            if (couchDb == null || version == null || vendorName == null)
            {
                throw new Exception($"Error while connecting to the database server: {response}");
            }
        }

        public void ProcessCreateDbMessage(HttpResponseMessage response, string databaseName)
        {
            var resultContentString = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var ok = (bool)resultContentString.SelectToken("ok");
                if (ok)
                {
                    Console.WriteLine($"Database {databaseName} was created successfully, result: {resultContentString}");
                }
                else
                {
                    throw new Exception($"Database was not created correctly: {resultContentString}");
                }
            }
            else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                var fileExists = (string)resultContentString.SelectToken("error") ?? "";
                if (fileExists.Equals("file_exists"))
                {
                    Console.WriteLine($"Database {databaseName} already exists.");
                }
                else
                {
                    throw new Exception($"Unexpected error while creating database: {resultContentString}");
                }
            }
        }
    }
}