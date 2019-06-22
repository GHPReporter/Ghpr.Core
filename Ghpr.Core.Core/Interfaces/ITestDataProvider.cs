using System;

namespace Ghpr.Core.Core.Interfaces
{
    public interface ITestDataProvider
    {
        Guid GetCurrentTestRunGuid();
        string GetCurrentTestRunFullName();
    }
}