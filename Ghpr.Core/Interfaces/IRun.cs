using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<string> TestRunFiles { get; set; }
        IRunInfo RunInfo { get; set; }
        string Name { get; set; }
        IRunSummary RunSummary { get; set; }
    }
}