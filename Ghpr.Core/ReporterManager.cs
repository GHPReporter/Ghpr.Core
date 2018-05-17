using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core
{
    public static class ReporterManager
    {
        private static bool _initialized;
        private static Reporter _reporter;
        private static readonly object Lock;

        public static string OutputPath => _reporter.GetReporterSettings().OutputPath;
        
        static ReporterManager()
        {
            Lock = new object();
            _initialized = false;
        }

        public static void Initialize(IDataService dataService)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(dataService);
                _initialized = true;
            }
        }

        public static void Initialize(ReporterSettingsDto settings, IDataService dataService)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(settings, dataService);
                _initialized = true;
            }
        }

        public static void Initialize(TestingFramework framework, IDataService dataService)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(framework, dataService);
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

        public static void TestStarted(TestRunDto testRun)
        {
            lock (Lock)
            {
                _reporter.TestStarted(testRun);
            }
        }

        public static void TestFinished(TestRunDto testRun)
        {
            lock (Lock)
            {
                _reporter.TestFinished(testRun);
            }
        }
    }
}