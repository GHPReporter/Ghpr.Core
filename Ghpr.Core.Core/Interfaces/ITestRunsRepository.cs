using Ghpr.Core.Core.Common;

namespace Ghpr.Core.Core.Interfaces
{
    public interface ITestRunsRepository
    {
        void OnRunStarted();
        TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun);
        void AddNewTestRun(TestRunDto testRun);
    }
}