﻿using System;
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
        string TestStackTrace { get; set; }
        string TestMessage { get; set; }
        string Result { get; set; }
        string TestType { get; set; }
        string Output { get; set; }
        string Priority { get; set; }
        string[] Categories { get; set; }
        ItemInfo TestInfo { get; set; }
        Guid RunGuid { get; set; }
        List<ITestScreenshot> Screenshots { get; set; }
        List<ITestEvent> Events { get; set; }
        List<ITestData> TestData { get; set; }

        TestResult TestResult { get; }
        bool FailedOrBroken { get; }
    }
}