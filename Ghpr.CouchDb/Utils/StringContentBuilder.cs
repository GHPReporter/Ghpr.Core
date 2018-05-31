using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Ghpr.CouchDb.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ghpr.CouchDb.Utils
{
    public class StringContentBuilder
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly Formatting _formatting;

        public StringContentBuilder()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            _formatting = Formatting.None;
        }

        public static StringContent Empty => new StringContent("");
        
        private StringContent CreateContentWithSelector(IEnumerable<KeyValuePair<string, JToken>> andDictionary)
        {
            var andArray = new JArray();
            foreach (var a in andDictionary)
                andArray.Add(new JObject(new JProperty(a.Key, a.Value)));
            var selector = new JObject(
                new JProperty("selector",
                    new JObject(
                        new JProperty("$and", new JArray(andArray))))
            );
            Console.WriteLine($"CONTENT: {selector}");
            var content = new StringContent(Serialize(selector), Encoding.UTF8, "application/json");
            return content;
        }

        public StringContent CreatePurgeContent(string docId, string[] docRevisions)
        {
            var selector = new JObject(
                new JProperty(docId, new JArray(docRevisions.Select(JToken.FromObject)))
            );
            var content = new StringContent(selector.ToString(), Encoding.UTF8, "application/json");
            return content;
        }

        public StringContent FindRunsContent(string runGuid)
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, JToken>>
            {
                new KeyValuePair<string, JToken>("type", EntityType.RunType),
                new KeyValuePair<string, JToken>("data.runInfo.guid", runGuid)
            });
        }

        public StringContent FindReportSettingsContent()
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, JToken>>
            {
                new KeyValuePair<string, JToken>("type", EntityType.ReportSettingsType)
            });
        }

        public StringContent FindTestRunsByGuid(string testGuid)
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, JToken>>
            {
                new KeyValuePair<string, JToken>("type", EntityType.TestRunType),
                new KeyValuePair<string, JToken>("data.testInfo.guid", testGuid)
            });
        }
        
        public StringContent FindTestScreenshotsByTestGuid(string testGuid, DateTime testStartDateTime, DateTime testFinishDateTime)
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, JToken>>
            {
                new KeyValuePair<string, JToken>( "type", EntityType.ScreenshotType),
                new KeyValuePair<string, JToken>( "data.testGuid", testGuid),
                new KeyValuePair<string, JToken>( "data.date", new JObject(new JProperty("$gt", testStartDateTime))),
                new KeyValuePair<string, JToken>( "data.date", new JObject(new JProperty("$lt", testFinishDateTime)))
            });
        }

        public string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item, _formatting, _jsonSerializerSettings);
        }

        public StringContent GetContent<T>(DatabaseEntity<T> entity)
        {
            return new StringContent(Serialize(entity));
        }
    }
}