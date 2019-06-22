using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Enums;
using Ghpr.Core.Core.Extensions;
using Ghpr.Core.Core.Helpers;
using Ghpr.Core.Core.Interfaces;
using Ghpr.Core.Core.Processors;
using Ghpr.Core.Core.Providers;
using Ghpr.Core.Core.Services;
using Ghpr.Core.Core.Settings;
using Ghpr.Core.Core.Utils;
using Newtonsoft.Json.Linq;

namespace Ghpr.Core.Core.Factories
{
    public static class ReporterFactory
    {
        public static IReporter Build(ITestDataProvider testDataProvider, string projectName = "")
        {
            return InitializeReporter(ReporterSettingsProvider.Load(Paths.Files.CoreSettings), testDataProvider, projectName);
        }

        public static IReporter Build(ReporterSettings settings, ITestDataProvider testDataProvider, string projectName = "")
        {
            return InitializeReporter(settings, testDataProvider, projectName);
        }

        public static IReporter Build(TestingFramework framework, ITestDataProvider testDataProvider, string projectName = "")
        {
            return InitializeReporter(ReporterSettingsProvider.Load(framework), testDataProvider, projectName);
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

        private static IReporter InitializeReporter(ReporterSettings settings, ITestDataProvider testDataProvider, string projectName = "")
        {
            ProjectSettings reporterProjectSettings;
            if (!string.IsNullOrEmpty(projectName) && settings.Projects.Any(s => projectName.Like(s.Pattern)))
            {
                var specificProjectSettings = settings.Projects.First(s => projectName.Like(s.Pattern));
                reporterProjectSettings =
                    specificProjectSettings.Settings.GetFromSourceOrDefault(settings.DefaultSettings);
            }
            else
            {
                reporterProjectSettings = settings.DefaultSettings;
            }
            if (reporterProjectSettings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(reporterProjectSettings.OutputPath),
                    "Reporter Output path must be specified! Please fix your .json settings file. " +
                    $"Your settings are: {JToken.FromObject(settings)}");
            }

            var logger = CreateInstanceFromFile<ILogger>(reporterProjectSettings.LoggerFile, new EmptyLogger());
            logger.SetUp(reporterProjectSettings);

            var dataWriterService = CreateInstanceFromFile<IDataWriterService>(reporterProjectSettings.DataServiceFile);
            dataWriterService.InitializeDataWriter(reporterProjectSettings, logger);

            var dataReaderService = CreateInstanceFromFile<IDataReaderService>(reporterProjectSettings.DataServiceFile);
            dataReaderService.InitializeDataReader(reporterProjectSettings, logger);

            CommonCache.Instance.InitializeDataReader(reporterProjectSettings, logger);
            CommonCache.Instance.InitializeDataWriter(reporterProjectSettings, logger);

            var actionHelper = new ActionHelper(logger);

            var reporter = new Reporter
            {
                Action = actionHelper,
                Logger = logger,
                TestDataProvider = testDataProvider,
                ReporterSettings = reporterProjectSettings,
                ReportSettings = new ReportSettingsDto(reporterProjectSettings.RunsToDisplay, 
                    reporterProjectSettings.TestsToDisplay, reporterProjectSettings.ReportName, reporterProjectSettings.ProjectName),
                DataWriterService = new DataWriterService(dataWriterService, CommonCache.Instance),
                DataReaderService = new DataReaderService(dataReaderService, CommonCache.Instance),
                RunRepository = new RunDtoRepository(),
                TestRunsRepository = new TestRunsRepository(),
                TestRunProcessor = new TestRunDtoProcessor(),
                ReportCleanUpProcessor = new ReportCleanUpProcessor(logger, actionHelper),
                TestRunStarted = false
            };
            return reporter;
        }
    }
}