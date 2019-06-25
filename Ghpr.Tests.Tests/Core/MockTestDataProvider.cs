using System;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Tests.Core
{
    public class MockTestDataProvider : ITestDataProvider
    {
        public Guid GetCurrentTestRunGuid()
        {
            return Guid.NewGuid();
        }

        public string GetCurrentTestRunFullName()
        {
            return "Test run full name";
        }
    }
}