using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core
{
    public static class ReporterManager
    {
        private static bool _initialized;
        private static Reporter _reporter;
        private static readonly object Lock;

        public static string OutputPath => _reporter.Settings.OutputPath;
        
        static ReporterManager()
        {
            Lock = new object();
            _initialized = false;
        }

        public static void Initialize()
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter();
                _initialized = true;
            }
        }

        public static void Initialize(IReporterSettings settings)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(settings);
                _initialized = true;
            }
        }

        public static void Initialize(TestingFramework framework)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(framework);
                _initialized = true;
            }
        }

        public static void RunStarted()
        {
            lock (Lock)
            {
                _reporter.RunStarted();
            }
        }

        public static void RunFinished()
        {
            lock (Lock)
            {
                _reporter.RunFinished();
            }
        }

        public static void TestStarted(ITestRun testRun)
        {
            lock (Lock)
            {
                _reporter.TestStarted(testRun);
            }
        }

        public static void TestFinished(ITestRun testRun)
        {
            lock (Lock)
            {
                _reporter.TestFinished(testRun);
            }
        }
    }
}