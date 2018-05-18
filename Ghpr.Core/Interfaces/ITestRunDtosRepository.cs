using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface ITestRunDtosRepository
    {
        void OnRunStarted();
        TestRunDto ExtractCorrespondingTestRun(TestRunDto finishedTestRun);
        void AddNewTestRun(TestRunDto testRun);
    }
}