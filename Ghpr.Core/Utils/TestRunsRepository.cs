using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
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

        public void UpdateCorrespondingTestRunWithScreenshot(Guid testGuid, string testFullName, TestScreenshotDto screenshotDto)
        {
            var testRun = new TestRunDto(testGuid, "", testFullName);
            var testRunByGuid = _currentTests.FirstOrDefault(t => t.TestInfo.Guid.Equals(testGuid) && !t.TestInfo.Guid.Equals(Guid.Empty));
            if (testRunByGuid != null)
            {
                testRunByGuid.Screenshots.Add(screenshotDto.TestScreenshotInfo);
            }
            else
            {
                var testRunByFullName = _currentTests.FirstOrDefault(t => t.FullName.Equals(testFullName));
                if (testRunByFullName != null)
                {
                    testRunByFullName.Screenshots.Add(screenshotDto.TestScreenshotInfo);
                }
                else
                {
                    testRun.Screenshots.Add(screenshotDto.TestScreenshotInfo);
                    _currentTests.Add(testRun);
                }
            }
        }

        public void AddNewTestRun(TestRunDto testRun)
        {
            _currentTests.Add(testRun);
        }
    }
}