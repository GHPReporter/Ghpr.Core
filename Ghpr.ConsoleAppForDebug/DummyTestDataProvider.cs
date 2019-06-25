using System;
using Ghpr.Core.Interfaces;

namespace Ghpr.ConsoleAppForDebug
{
    public class DummyTestDataProvider : ITestDataProvider
    {
        public Guid GetCurrentTestRunGuid()
        {
            return Guid.NewGuid();
        }

        public string GetCurrentTestRunFullName()
        {
            return "some test name";
        }
    }
}