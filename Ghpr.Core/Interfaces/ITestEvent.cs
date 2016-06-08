using System;

namespace Ghpr.Core.Interfaces
{
    public interface ITestEvent
    {
        string Name { get; set; }
        DateTime Started { get; set; }
        DateTime Finished { get; set; }
    }
}