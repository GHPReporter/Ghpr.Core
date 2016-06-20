using System.Collections.Generic;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Extensions
{
    public static class TestRunListExtensions
    {
        public static List<ITestRun> RemoveTest(this List<ITestRun> tests, ITestRun test)
        {
            var testToRemove = tests.GetTest(test);
            tests.Remove(testToRemove);
            return tests;
        }
    }
}