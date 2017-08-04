using System;
using System.Collections.Generic;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestRun : ITestRun
    {
        public TestRun(Guid guid, string name = "", string fullName = "")
        {
            TestInfo = new ItemInfo
            {
                Guid = guid,
                Start = DateTime.Now,
                Finish = default(DateTime)
            };
            Name = name;
            FullName = fullName;
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Output = "";
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<ITestScreenshot>();
            Events = new List<ITestEvent>();
        }

        public TestRun(string guid = "", string name = "", string fullName = "")
        {
            TestInfo = new ItemInfo
            {
                Guid = guid.Equals("") ? Guid.Empty : Guid.Parse(guid),
                Start = DateTime.Now,
                Finish = default(DateTime)
            };
            Name = name;
            FullName = fullName;
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Output = "";
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<ITestScreenshot>();
            Events = new List<ITestEvent>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

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
        public List<ITestScreenshot> Screenshots { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<ITestEvent> Events { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<ITestData> TestData { get; set; }

        public bool FailedOrBroken => TestResult.Equals(TestResult.Broken) || TestResult.Equals(TestResult.Failed);

        public TestResult TestResult
        {
            get
            {
                if (Result.Contains("Passed"))
                {
                    return TestResult.Passed;
                }
                if (Result.Contains("Error") || Result.Contains("Broken"))
                {
                    return TestResult.Broken;
                }
                if (Result.Contains("Failed") || Result.Contains("Failure"))
                {
                    return TestResult.Failed;
                }
                if (Result.Contains("Inconclusive"))
                {
                    return TestResult.Inconclusive;
                }
                if (Result.Contains("Ignored") || Result.Contains("Skipped") || Result.Contains("NotExecuted"))
                {
                    return TestResult.Ignored;
                }
                return TestResult.Unknown;
            }
        }
    }
}
