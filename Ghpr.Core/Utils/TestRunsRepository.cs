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
        private List<TestScreenshotDto> _currentScreenshots;

        public void OnRunStarted()
        {
            _currentTests = new List<TestRunDto>();
            _currentScreenshots = new List<TestScreenshotDto>();
        }

        public TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun)
        {
            var testRun = _currentTests.FirstOrDefault(t => t.TestInfo.Guid.Equals(finishedTestRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
                          ?? _currentTests.FirstOrDefault(t => t.FullName.Equals(finishedTestRun.FullName))
                          ?? new TestRunDto();
            _currentTests.Remove(testRun);
            var correspondingScreenshots =
                _currentScreenshots.Where(s => s.TestGuid.Equals(finishedTestRun.TestInfo.Guid));
            testRun.Screenshots.AddRange(correspondingScreenshots);
            _currentScreenshots.ForEach(s => _currentScreenshots.Remove(s));
            return testRun;
        }

        public void AddNewTestRun(TestRunDto testRun)
        {
            _currentTests.Add(testRun);
        }

        public void AddNewScreenshot(TestScreenshotDto testScreenshot)
        {
            _currentScreenshots.Add(testScreenshot);
        }
    }
}