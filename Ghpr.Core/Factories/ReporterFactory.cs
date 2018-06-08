﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Processors;
using Ghpr.Core.Providers;
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

        private static T CreateInstanceFromFile<T>(string fileName) where T : class
        {
            var uri = new Uri(typeof(ReporterFactory).Assembly.CodeBase);
            var dataServiceAssemblyFullPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "", fileName);
            var dataServiceAssembly = Assembly.LoadFrom(dataServiceAssemblyFullPath);
            var implementationType = dataServiceAssembly.GetTypes()
                .FirstOrDefault(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            if (implementationType == null)
            {
                throw new NullReferenceException($"Can't find implementation of {nameof(T)} in {fileName} file. " +
                                                 "Please fix your .json settings file.");
            }
            var instance = Activator.CreateInstance(implementationType) as T;
            if (instance == null)
            {
                throw new NullReferenceException($"Can't find create instance of type {nameof(implementationType)} from {fileName} file. " +
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

            var logger = CreateInstanceFromFile<ILogger>(settings.LoggerFile);
            logger.SetUp(settings);

            var dataService = CreateInstanceFromFile<IDataService>(settings.DataServiceFile);
            dataService.Initialize(settings, logger);
            
            var reporter = new Reporter
            {
                Action = new ActionHelper(logger),
                Logger = logger,
                TestDataProvider = testDataProvider,
                ReporterSettings = settings,
                ReportSettings = new ReportSettingsDto(settings.RunsToDisplay, settings.TestsToDisplay),
                DataService = dataService,
                RunRepository = new RunDtoRepository(),
                TestRunsRepository = new TestRunsRepository(),
                TestRunProcessor = new TestRunDtoProcessor(),
                TestRunStarted = false
            };
            return reporter;
        }
    }
}