using System;
using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<string> TestRunFiles { get; set; }
        Guid Guid { get; }
        string Name { get; set; }
        IRunSummary RunSummary { get; set; }
        DateTime Start { get; set; }
        DateTime Finish { get; set; }
    }
}