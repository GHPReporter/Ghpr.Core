using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IRuns
    {
        List<IRunInfo> RunsInfo { get; set; }
    }
}