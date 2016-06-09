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
                switch (TestRunResult)
                {
                    case TestRunResult.Ignored:
                        return Colors.TestIgnored;

                    case TestRunResult.Passed:
                        return Colors.TestPassed;

                    case TestRunResult.Broken:
                        return Colors.TestBroken;

                    case TestRunResult.Inconclusive:
                        return Colors.TestInconclusive;

                    case TestRunResult.Failed:
                        return Colors.TestFailed;

                    case TestRunResult.Unknown:
                        return Colors.TestUnknown;

                    default:
                        return Colors.TestUnknown;
                }
            }
        }

        public TestRunResult TestRunResult
        {
            get
            {
                if (Result.Contains("Passed"))
                {
                    return TestRunResult.Passed;
                }
                if (Result.Contains("Failed") || Result.Contains("Failure"))
                {
                    return TestRunResult.Failed;
                }
                if (Result.Contains("Error"))
                {
                    return TestRunResult.Broken;
                }
                if (Result.Contains("Inconclusive"))
                {
                    return TestRunResult.Inconclusive;
                }
                if (Result.Contains("Ignored") || Result.Contains("Skipped"))
                {
                    return TestRunResult.Ignored;
                }
                return TestRunResult.Unknown;
            }
        }
    }
}