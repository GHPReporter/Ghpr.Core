using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Interfaces;

namespace Ghpr.Core.Core.Utils
{
    public class TestRunsRepository : ITestRunsRepository
    {
        private List<TestRunDto> _currentTests;

        public void OnRunStarted()
        {
            _currentTests = new List<TestRunDto>();
        }

        public TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun)
        {
            var testRun = new TestRunDto();
            var testRunByGuid = _currentTests.FirstOrDefault(t => t.TestInfo.Guid.Equals(finishedTestRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty));
            if (testRunByGuid != null)
            {
                testRun = testRunByGuid;
                _currentTests.Remove(testRunByGuid);
            }
            else
            {
                var testRunByFullName = _currentTests.FirstOrDefault(t => t.FullName.Equals(finishedTestRun.FullName));
                if (testRunByFullName != null)
                {
                    testRun = testRunByFullName;
                    _currentTests.Remove(testRunByFullName);
                }
            }
            return testRun;
        }

        public void AddNewTestRun(TestRunDto testRun)
        {
            _currentTests.Add(testRun);
        }
    }
}