using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Processors;
using Ghpr.Core.Providers;
using Ghpr.Core.Services;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Factories
{
    public static class ReporterFactory
    {
        public static IReporter Build(ITestDataProvider testDataProvider)
        {
            return InitializeReporter(ReporterSettingsProvider.Load(Paths.Files.CoreSettings), testDataProvider);
        }

        public static IReporter Build(ReporterSettings settings, ITestDataProvider testDataProvider)
        {
            return InitializeReporter(settings, testDataProvider);
        }

        public static IReporter Build(TestingFramework framework, ITestDataProvider testDataProvider)
        {
            return InitializeReporter(ReporterSettingsProvider.Load(framework), testDataProvider);
        }

        private static T CreateInstanceFromFile<T>(string fileName, T defaultImplementation = null) where T : class
        {
            if (fileName.Equals("") && defaultImplementation != null)
            {
                return defaultImplementation;
            }
            var uri = new Uri(typeof(ReporterFactory).Assembly.CodeBase);
            var instanceAssemblyFullPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "", fileName);
            var instanceAssembly = Assembly.LoadFrom(instanceAssemblyFullPath);
            Type implementationType;
            try
            {
                implementationType = instanceAssembly.GetTypes().FirstOrDefault(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (var exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    var exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                var errorMessage = sb.ToString();
                //Display or log the error based on your application.
                throw new Exception(errorMessage);
            }
            if (implementationType == null)
            {
                throw new NullReferenceException($"Can't find implementation of {typeof(T)} in {fileName} file. " +
                                                 "Please fix your .json settings file.");
            }
            var instance = Activator.CreateInstance(implementationType) as T;
            if (instance == null)
            {
                throw new NullReferenceException($"Can't find create instance of type {implementationType} from {fileName} file. " +
                                                 "Please fix your .json settings file.");
            }
            return instance;
        }

        private static IReporter InitializeReporter(ReporterSettings settings, ITestDataProvider testDataProvider)
        {
            if (settings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(settings.OutputPath),
                    "Reporter Output path must be specified. Please fix your .json settings file.");
            }

            var logger = CreateInstanceFromFile<ILogger>(settings.LoggerFile, new EmptyLogger());
            logger.SetUp(settings);

            var dataWriterService = CreateInstanceFromFile<IDataWriterService>(settings.DataServiceFile);
            dataWriterService.InitializeDataWriter(settings, logger);

            var dataReaderService = CreateInstanceFromFile<IDataReaderService>(settings.DataServiceFile);
            dataReaderService.InitializeDataReader(settings, logger);

            CommonCache.Instance.InitializeDataReader(settings, logger);
            CommonCache.Instance.InitializeDataWriter(settings, logger);

            var reporter = new Reporter
            {
                Action = new ActionHelper(logger),
                Logger = logger,
                TestDataProvider = testDataProvider,
                ReporterSettings = settings,
                ReportSettings = new ReportSettingsDto(settings.RunsToDisplay, settings.TestsToDisplay),
                DataWriterService = new DataWriterService(dataWriterService, CommonCache.Instance),
                DataReaderService = new DataReaderService(dataReaderService, CommonCache.Instance),
                RunRepository = new RunDtoRepository(),
                TestRunsRepository = new TestRunsRepository(),
                TestRunProcessor = new TestRunDtoProcessor(),
                TestRunStarted = false
            };
            return reporter;
        }
    }
}