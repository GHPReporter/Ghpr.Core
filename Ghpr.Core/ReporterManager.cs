using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core
{
    public static class ReporterManager
    {
        private static readonly Reporter Reporter;
        private static readonly object Lock;

        public static string OutputPath => Reporter.Settings.OutputPath;

        static ReporterManager()
        {
            Lock = new object();
            Reporter = new Reporter(TestingFramework.SpecFlow);
        }
        
        public static void RunStarted()
        {
            lock (Lock)
            {
                Reporter.RunStarted();
            }
        }

        public static void RunFinished()
        {
            lock (Lock)
            {
                Reporter.RunFinished();
            }
        }

        public static void TestStarted(ITestRun testRun)
        {
            lock (Lock)
            {
                Reporter.TestStarted(testRun);
            }
        }

        public static void TestFinished(ITestRun testRun)
        {
            lock (Lock)
            {
                Reporter.TestFinished(testRun);
            }
        }
    }
}