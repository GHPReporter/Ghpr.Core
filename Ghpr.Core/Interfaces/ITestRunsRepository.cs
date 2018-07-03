using System;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface ITestRunsRepository
    {
        void OnRunStarted();
        TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun);
        void UpdateCorrespondingTestRunWithScreenshot(Guid testGuid, string testFullName, TestScreenshotDto screenshotDto);
        void AddNewTestRun(TestRunDto testRun);
    }
}