using System;
using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<ITestRun> TestRuns { get; set; }
        Guid Guid { get; }
    }
}