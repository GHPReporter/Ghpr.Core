using System;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Enums;
using Ghpr.Core.Core.Factories;
using Ghpr.Core.Core.Interfaces;
using Ghpr.Core.Core.Settings;

namespace Ghpr.Core.Core
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

        public static void Initialize(ITestDataProvider testDataProvider, string projectName = "")
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = ReporterFactory.Build(testDataProvider, projectName);
                _initialized = true;
            }
        }

        public static void Initialize(ReporterSettings settings, ITestDataProvider testDataProvider, string projectName = "")
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = ReporterFactory.Build(settings, testDataProvider, projectName);
                _initialized = true;
            }
        }

        public static void Initialize(TestingFramework framework, ITestDataProvider testDataProvider, string projectName = "")
        {
            lock (Lock)
            {
                if (_initialized) return;
                _reporter = ReporterFactory.Build(framework, testDataProvider, projectName);
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

        public static void TestFinished(TestRunDto testRun, TestOutputDto testOutputDto)
        {
            lock (Lock)
            {
                _reporter.TestFinished(testRun, testOutputDto);
            }
        }

        public static void SaveScreenshot(byte[] screenBytes, string format)
        {
            lock (Lock)
            {
                _reporter.SaveScreenshot(screenBytes, format);
            }
        }

        public static void SetTestDataProvider(ITestDataProvider testDataProvider)
        {
            _reporter.SetTestDataProvider(testDataProvider);
        }

        public static void Action(Action<IReporter> action)
        {
            lock (Lock)
            {
                action.Invoke(_reporter);
            }
        }

        public static void TearDown()
        {
            lock (Lock)
            {
                _reporter?.TearDown();
            }
            _reporter = null;
            _initialized = false;
        }
    }
}