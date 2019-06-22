using System;

namespace Ghpr.Core.Interfaces
{
    public interface ITestDataProvider
    {
        Guid GetCurrentTestRunGuid();
        string GetCurrentTestRunFullName();
    }
}