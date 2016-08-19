using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<string> TestRunFiles { get; set; }
        ItemInfo RunInfo { get; set; }
        string Name { get; set; }
        string Sprint { get; set; }
        IRunSummary RunSummary { get; set; }
    }
}