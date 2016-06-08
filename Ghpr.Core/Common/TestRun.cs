using System;
using System.Collections.Generic;
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
                switch (Result)
                {
                    case "Ignored":
                        return Colors.TestIgnored;
                    case "Skipped:Ignored":
                        return Colors.TestIgnored;

                    case "Passed":
                        return Colors.TestPassed;
                    case "Success":
                        return Colors.TestPassed;

                    case "Failed:Error":
                        return Colors.TestBroken;
                    case "Error":
                        return Colors.TestBroken;

                    case "Inconclusive":
                        return Colors.TestInconclusive;

                    case "Failure":
                        return Colors.TestFailed;
                    case "Failed":
                        return Colors.TestFailed;

                    default:
                        return Colors.TestUnknown;
                }
            }
        }
    }
}