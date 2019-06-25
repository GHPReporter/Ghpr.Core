using System;
using Ghpr.Core.Core.Interfaces;

namespace Ghpr.Tests.Core
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