using System;
using System.Collections.Generic;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Common
{
    public class TestRun : ITestRun
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public double TestDuration { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeFinish { get; set; }
        public string TestStackTrace { get; set; }
        public string TestMessage { get; set; }
        public string Result { get; set; }
        public Guid Guid { get; set; }
        public List<ITestScreenshot> Screenshots { get; set; }
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