using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core
{
    public static class ReporterManager
    {
        private static bool _initialized;
        private static Reporter _reporter;
        private static readonly object Lock;

        public static string OutputPath => _reporter.Settings.OutputPath;

        //private static readonly Log Log;

        static ReporterManager()
        {
            Lock = new object();
            _initialized = false;
            //Log = new Log(@"C:\_GHPReporter_SpecFlow_Report");
            //Log.Write("Constr");
        }

        public static void Initialize()
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter();
                _initialized = true;
                //Log.Write("Init");
            }
        }

        public static void Initialize(IReporterSettings settings)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(settings);
                _initialized = true;
                //Log.Write("Init");
            }
        }

        public static void Initialize(TestingFramework framework)
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = new Reporter(framework);
                _initialized = true;
                //Log.Write("Init");
            }
        }

        public static void RunStarted()
        {
            lock (Lock)
            {
                _reporter.RunStarted();
                //Log.Write("Run started");
            }
        }

        public static void RunFinished()
        {
            lock (Lock)
            {
                _reporter.RunFinished();
                //Log.Write("Run finished");
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