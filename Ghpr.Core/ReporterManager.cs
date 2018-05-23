using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Factories;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core
{
    public static class ReporterManager
    {
        private static bool _initialized;
        private static IReporter _reporter;
        private static readonly object Lock;

        public static string OutputPath => _reporter.ReporterSettings.OutputPath;
        
        static ReporterManager()
        {
            Lock = new object();
            _initialized = false;
        }

        public static void Initialize(ITestDataProvider testDataProvider)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = ReporterFactory.Build(testDataProvider);
                _initialized = true;
            }
        }

        public static void Initialize(ReporterSettings settings, ITestDataProvider testDataProvider)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = ReporterFactory.Build(settings, testDataProvider);
                _initialized = true;
            }
        }

        public static void Initialize(TestingFramework framework, ITestDataProvider testDataProvider)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = ReporterFactory.Build(framework, testDataProvider);
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