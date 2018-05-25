using System;
using System.Collections.Generic;
using Ghpr.Core.Enums;
using Newtonsoft.Json;

namespace Ghpr.CouchDb.Entities
{
    public class TestRun
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public double TestDuration { get; set; }
        
        [JsonProperty(PropertyName = "testStackTrace")]
        public string TestStackTrace { get; set; }

        [JsonProperty(PropertyName = "testMessage")]
        public string TestMessage { get; set; }

        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "output")]
        public string Output { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public string[] Categories { get; set; }

        [JsonProperty(PropertyName = "testInfo")]
        public ItemInfo TestInfo { get; set; }

        [JsonProperty(PropertyName = "runGuid")]
        public Guid RunGuid { get; set; }

        [JsonProperty(PropertyName = "screenshots")]
        public List<TestScreenshot> Screenshots { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<TestEvent> Events { get; set; }

        [JsonProperty(PropertyName = "testData")]
        public List<TestData> TestData { get; set; }

        public bool FailedOrBroken => TestResult.Equals(TestResult.Broken) || TestResult.Equals(TestResult.Failed);

        public TestResult TestResult
        {
            get
            {
                if (Result.ToLowerInvariant().Contains("passed"))
                {
                    return TestResult.Passed;
                }
                if (Result.ToLowerInvariant().Contains("error") || Result.ToLowerInvariant().Contains("broken"))
                {
                    return TestResult.Broken;
                }
                if (Result.ToLowerInvariant().Contains("failed") || Result.ToLowerInvariant().Contains("failure"))
                {
                    return TestResult.Failed;
                }
                if (Result.ToLowerInvariant().Contains("inconclusive"))
                {
                    return TestResult.Inconclusive;
                }
                if (Result.ToLowerInvariant().Contains("ignored") || Result.ToLowerInvariant().Contains("skipped") 
                    || Result.ToLowerInvariant().Contains("notexecuted"))
                {
                    return TestResult.Ignored;
                }
                return TestResult.Unknown;
            }
        }
    }
}
