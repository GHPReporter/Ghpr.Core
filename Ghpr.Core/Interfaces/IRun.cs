using System;
using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<TestRun> TestRuns { get; set; }
        Guid Guid { get; }
    }
}