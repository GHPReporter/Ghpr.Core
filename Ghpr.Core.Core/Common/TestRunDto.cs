using System;
using System.Collections.Generic;
using Ghpr.Core.Enums;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    public class TestRunDto
    {
        [JsonConstructor]
        public TestRunDto()
        {
            TestInfo = new ItemInfoDto
            {
                Guid = Guid.Empty,
                Start = DateTime.Now,
                Finish = default(DateTime)
            };
            Name = "";
            FullName = "";
            Description = "";
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Output = new SimpleItemInfoDto();
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<SimpleItemInfoDto>();
            Events = new List<TestEventDto>();
            TestData = new List<TestDataDto>();
        }

        public TestRunDto(Guid guid, string name = "", string fullName = "")
        {
            TestInfo = new ItemInfoDto
            {
                Guid = guid,
                Start = default(DateTime),
                Finish = default(DateTime)
            };
            Name = name;
            FullName = fullName;
            Description = "";
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Duration = 0.0;
            Output = new SimpleItemInfoDto();
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<SimpleItemInfoDto>();
            Events = new List<TestEventDto>();
            TestData = new List<TestDataDto>();
        }

        public TestRunDto(string guid = "", string name = "", string fullName = "")
        {
            TestInfo = new ItemInfoDto
            {
                Guid = guid.Equals("") ? Guid.Empty : Guid.Parse(guid),
                Start = default(DateTime),
                Finish = default(DateTime)
            };
            Name = name;
            FullName = fullName;
            Description = "";
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Duration = 0.0;
            Output = new SimpleItemInfoDto();
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<SimpleItemInfoDto>();
            Events = new List<TestEventDto>();
            TestData = new List<TestDataDto>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public double Duration { get; set; }

        [JsonProperty(PropertyName = "testStackTrace")]
        public string TestStackTrace { get; set; }

        [JsonProperty(PropertyName = "testMessage")]
        public string TestMessage { get; set; }

        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "output")]
        public SimpleItemInfoDto Output { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public string[] Categories { get; set; }

        [JsonProperty(PropertyName = "testInfo")]
        public ItemInfoDto TestInfo { get; set; }

        [JsonProperty(PropertyName = "runGuid")]
        public Guid RunGuid { get; set; }

        [JsonProperty(PropertyName = "screenshots")]
        public List<SimpleItemInfoDto> Screenshots { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<TestEventDto> Events { get; set; }

        [JsonProperty(PropertyName = "testData")]
        public List<TestDataDto> TestData { get; set; }

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
