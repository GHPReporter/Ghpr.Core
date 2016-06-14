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
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string FullName { get; set; }
        [JsonProperty]
        public double TestDuration { get; set; }
        [JsonProperty]
        public DateTime DateTimeStart { get; set; }
        [JsonProperty]
        public DateTime DateTimeFinish { get; set; }
        [JsonProperty]
        public string TestStackTrace { get; set; }
        [JsonProperty]
        public string TestMessage { get; set; }
        [JsonProperty]
        public string Result { get; set; }
        [JsonProperty]
        public Guid Guid { get; set; }
        [JsonProperty]
        public List<TestScreenshot> Screenshots { get; set; }
        [JsonProperty]
        public List<TestEvent> Events { get; set; }

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