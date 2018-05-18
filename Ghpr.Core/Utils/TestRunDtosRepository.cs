using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class TestRunDtosRepository : ITestRunDtosRepository
    {
        private List<TestRunDto> _currentTests;

        public void OnRunStarted()
        {
            _currentTests = new List<TestRunDto>();
        }

        public TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun)
        {
            var testRun = _currentTests.FirstOrDefault(t => t.TestInfo.Guid.Equals(finishedTestRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
                          ?? _currentTests.FirstOrDefault(t => t.FullName.Equals(finishedTestRun.FullName))
                          ?? new TestRunDto();
            _currentTests.Remove(testRun);
            return testRun;
        }

        public void AddNewTestRun(TestRunDto testRun)
        {
            _currentTests.Add(testRun);
        }
    }
}