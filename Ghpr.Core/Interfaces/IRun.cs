using System;
using System.Collections.Generic;

namespace Ghpr.Core.Interfaces
{
    public interface IRun
    {
        List<string> TestRunFiles { get; set; }
        Guid Guid { get; }
    }
}