﻿using System;
using System.Collections.Generic;
using Ghpr.Core.Enums;

namespace Ghpr.Core.Common
{
    public class TestRunDto
    {
        public TestRunDto(Guid guid, string name = "", string fullName = "")
        {
            TestInfo = new ItemInfoDto
            {
                Guid = guid,
                Start = DateTime.Now,
                Finish = default(DateTime)
            };
            Name = name;
            FullName = fullName;
            Description = "";
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Output = "";
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<TestScreenshotDto>();
            Events = new List<TestEventDto>();
            TestData = new List<TestDataDto>();
        }

        public TestRunDto(string guid = "", string name = "", string fullName = "")
        {
            TestInfo = new ItemInfoDto
            {
                Guid = guid.Equals("") ? Guid.Empty : Guid.Parse(guid),
                Start = DateTime.Now,
                Finish = default(DateTime)
            };
            Name = name;
            FullName = fullName;
            Description = "";
            TestStackTrace = "";
            TestMessage = "";
            Result = "";
            Output = "";
            Priority = "";
            TestType = "";
            Categories = new string[] { };
            RunGuid = Guid.Empty;
            Screenshots = new List<TestScreenshotDto>();
            Events = new List<TestEventDto>();
            TestData = new List<TestDataDto>();
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string TestStackTrace { get; set; }
        public string TestMessage { get; set; }
        public string Result { get; set; }
        public string TestType { get; set; }
        public string Output { get; set; }
        public string Priority { get; set; }
        public string[] Categories { get; set; }
        public ItemInfoDto TestInfo { get; set; }
        public Guid RunGuid { get; set; }
        public List<TestScreenshotDto> Screenshots { get; set; }
        public List<TestEventDto> Events { get; set; }
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
