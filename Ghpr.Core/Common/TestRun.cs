using System;
using System.Collections.Generic;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestRun : ITestRun
    {
        public TestRun(string guid = "", string name = "", string fullName = "")
        {
            Guid = guid.Equals("") ? Guid.Empty : Guid.Parse(guid);
            DateTimeStart = DateTime.Now;
            DateTimeFinish = default(DateTime);
            Name = name;
            FullName = fullName;
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Screenshots = new List<ITestScreenshot>();
            Events = new List<ITestEvent>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        
        public double TestDuration => (DateTimeFinish - DateTimeStart).TotalSeconds;

        [JsonProperty(PropertyName = "dateTimeStart")]
        public DateTime DateTimeStart { get; set; }

        [JsonProperty(PropertyName = "dateTimeFinish")]
        public DateTime DateTimeFinish { get; set; }

        [JsonProperty(PropertyName = "testStackTrace")]
        public string TestStackTrace { get; set; }

        [JsonProperty(PropertyName = "testMessage")]
        public string TestMessage { get; set; }

        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }

        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "screenshots")]
        public List<ITestScreenshot> Screenshots { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<ITestEvent> Events { get; set; }

        public string TestRunColor
        {
            get
            {
                switch (TestResult)
                {
                    case TestResult.Ignored:
                        return Colors.TestIgnored;

                    case TestResult.Passed:
                        return Colors.TestPassed;

                    case TestResult.Broken:
                        return Colors.TestBroken;

                    case TestResult.Inconclusive:
                        return Colors.TestInconclusive;

                    case TestResult.Failed:
                        return Colors.TestFailed;

                    case TestResult.Unknown:
                        return Colors.TestUnknown;

                    default:
                        return Colors.TestUnknown;
                }
            }
        }

        public bool FailedOrBroken => TestResult.Equals(TestResult.Broken) || TestResult.Equals(TestResult.Failed);

        public TestResult TestResult
        {
            get
            {
                if (Result.Contains("Passed"))
                {
                    return TestResult.Passed;
                }
                if (Result.Contains("Failed") || Result.Contains("Failure"))
                {
                    return TestResult.Failed;
                }
                if (Result.Contains("Error"))
                {
                    return TestResult.Broken;
                }
                if (Result.Contains("Inconclusive"))
                {
                    return TestResult.Inconclusive;
                }
                if (Result.Contains("Ignored") || Result.Contains("Skipped"))
                {
                    return TestResult.Ignored;
                }
                return TestResult.Unknown;
            }
        }
    }
}