using System;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Tests.Core
{
    public class MockTestDataProviderWithException : ITestDataProvider
    {
        public Guid GetCurrentTestRunGuid()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentTestRunFullName()
        {
            throw new NotImplementedException();
        }
    }
}