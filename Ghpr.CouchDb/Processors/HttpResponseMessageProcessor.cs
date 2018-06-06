using System;
using System.Net;
using System.Net.Http;
using Ghpr.Core.Interfaces;
using Ghpr.CouchDb.Entities;
using Ghpr.CouchDb.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ghpr.CouchDb.Processors
{
    public class HttpResponseMessageProcessor
    {
        private readonly ILogger _logger;

        public HttpResponseMessageProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void ProcessScreenshotSavedMessage(HttpResponseMessage response, string testGuid, DateTime screenshotDateTime)
        {
            var postResString = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)postResString.SelectToken("ok"))
            {
                _logger.Info($"Screenshot for test {testGuid} with date time {screenshotDateTime} " +
                                  $"was saved successfully, result: {postResString}");
            }
            else
            {
                _logger.Warn($"Screenshot was not saved correctly: {postResString}");
            }
        }

        public void ProcessReportSettingsSavedMessage(HttpResponseMessage response, ReportSettings reportSettings)
        {
            var postResString = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)postResString.SelectToken("ok"))
            {
                _logger.Info($"Report settings {JsonConvert.SerializeObject(reportSettings, Formatting.Indented)} " +
                                  $"were saved successfully, result: {postResString}");
            }
            else
            {
                _logger.Warn($"Report settings were not saved correctly: {postResString}");
            }
        }

        public void ProcessTestRunSavedMessage(HttpResponseMessage response, ItemInfo itemInfo)
        {
            var jContent = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)jContent.SelectToken("ok"))
            {
                _logger.Info($"Test run {JsonConvert.SerializeObject(itemInfo, Formatting.Indented)} " +
                                  $"was created successfully, result: {jContent}");
            }
            else
            {
                _logger.Warn($"Test run was not saved correctly: {jContent}");
            }
        }

        public void ProcessRunSavedMessage(HttpResponseMessage response, ItemInfo itemInfo)
        {
            var jContent = response.ContentAsJObject();
            if (response.StatusCode == HttpStatusCode.Created && (bool)jContent.SelectToken("ok"))
            {
                _logger.Info($"Run {JsonConvert.SerializeObject(itemInfo, Formatting.Indented)} " +
                                  $"was saved successfully, result: {jContent}");
            }
            else
            {
                _logger.Warn($"Run was not saved correctly: {jContent}");
            }
        }

        public void ProcessValidateConnectionMessage(string response)
        {
            var r = JObject.Parse(response);
            var couchDb = (string)r.SelectToken("couchdb");
            var version = (string)r.SelectToken("version");
            var vendorName = (string)r.SelectToken("vendor").SelectToken("name");
            if (couchDb == null || version == null || vendorName == null)
            {
                var exception = new Exception($"Error while connecting to the database server: {response}");
                _logger.Fatal("Can't connect to the database", exception);
                throw exception;
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
                    _logger.Info($"Database {databaseName} was created successfully, result: {resultContentString}");
                }
                else
                {
                    var exception = new Exception($"Database was not created correctly: {resultContentString}");
                    _logger.Fatal("Can't create the database", exception);
                    throw exception;
                }
            }
            else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                var fileExists = (string)resultContentString.SelectToken("error") ?? "";
                if (fileExists.Equals("file_exists"))
                {
                    _logger.Info($"Database {databaseName} already exists.");
                }
                else
                {
                    var exception = new Exception($"Unexpected error while creating database: {resultContentString}");
                    _logger.Fatal("Can't create the database", exception);
                    throw exception;
                }
            }
        }
    }
}