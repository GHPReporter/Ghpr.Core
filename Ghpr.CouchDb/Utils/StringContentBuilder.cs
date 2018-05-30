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
        public static StringContent Empty => new StringContent("");

        private static StringContent CreateContentWithSelector(List<KeyValuePair<string, string>> andDictionary)
        {
            var andArray = new JArray();
            foreach (var a in andDictionary)
                andArray.Add(new JObject(new JProperty(a.Key, a.Value)));
            var selector = new JObject(
                new JProperty("selector",
                    new JObject(
                        new JProperty("$and", new JArray(andArray))))
            );
            var content = new StringContent(selector.ToString(), Encoding.UTF8, "application/json");
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
            return CreateContentWithSelector(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("type", EntityType.RunType),
                new KeyValuePair<string, string>("data.runInfo.guid", runGuid)
            });
        }

        public StringContent FindReportSettingsContent()
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("type", EntityType.ReportSettingsType)
            });
        }

        public StringContent FindTestRunsByGuid(string testGuid)
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("type", EntityType.TestRunType),
                new KeyValuePair<string, string>("data.testInfo.guid", testGuid)
            });
        }
        
        public StringContent FindTestScreenshotsByTestGuid(string testGuid, DateTime testStarDateTime, DateTime testFinishDateTime)
        {
            return CreateContentWithSelector(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "type", EntityType.ScreenshotType),
                new KeyValuePair<string, string>( "data.testGuid", testGuid),
                new KeyValuePair<string, string>( "data.date", $"{{\"$gt\":{testStarDateTime}}}"),
                new KeyValuePair<string, string>( "data.date", $"{{\"$lt\":{testFinishDateTime}}}")
            });
        }

        public StringContent GetContent<T>(DatabaseEntity<T> entity)
        {
            return new StringContent(JsonConvert.SerializeObject(entity, Formatting.None,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }
    }
}