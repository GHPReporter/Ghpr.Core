using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface ITestRunsRepository
    {
        void OnRunStarted();
        TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun);
        void AddNewTestRun(TestRunDto testRun);
    }
}