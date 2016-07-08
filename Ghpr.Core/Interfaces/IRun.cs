using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<string> TestRunFiles { get; set; }
        IItemInfo RunInfo { get; set; }
        string Name { get; set; }
        string Sprint { get; set; }
        IRunSummary RunSummary { get; set; }
    }
}