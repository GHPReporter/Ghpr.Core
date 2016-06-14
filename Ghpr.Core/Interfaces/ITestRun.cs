using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;

namespace Ghpr.Core.Interfaces
{
    public interface ITestRun
    {
        string Name { get; set; }
        string FullName { get; set; }
        double TestDuration { get; set; }
        DateTime DateTimeStart { get; set; }
        DateTime DateTimeFinish { get; set; }
        string TestStackTrace { get; set; }
        string TestMessage { get; set; }
        string Result { get; set; }
        Guid Guid { get; set; }
        List<TestScreenshot> Screenshots { get; set; }
        List<TestEvent> Events { get; set; }

        string TestRunColor { get; }
        TestResult TestResult { get; }
    }
}