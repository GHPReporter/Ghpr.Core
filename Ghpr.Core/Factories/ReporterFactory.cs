using System;
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
        public static IReporter Build(IScreenshotService screenshotService)
        {
            return InitializeReporter(ReporterSettingsProvider.Load(), screenshotService);
        }

        public static IReporter Build(ReporterSettings settings, IScreenshotService screenshotService)
        {
            return InitializeReporter(settings, screenshotService);
        }

        public static IReporter Build(TestingFramework framework, IScreenshotService screenshotService)
        {
            return InitializeReporter(ReporterSettingsProvider.Load(framework), screenshotService);
        }

        private static IReporter InitializeReporter(ReporterSettings settings, IScreenshotService screenshotService)
        {
            if (settings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(settings.OutputPath),
                    "Reporter Output path must be specified. Please fix your .json settings file.");
            }

            var dataServiceAssembly = Assembly.LoadFile(settings.DataServiceFile);
            var dataServiceType = dataServiceAssembly.GetTypes()
                .FirstOrDefault(t => typeof(IDataService).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            if (dataServiceType == null)
            {
                throw new NullReferenceException($"Can't find implementation of {nameof(IDataService)} in {settings.DataServiceFile} file. " +
                                                 "Please fix your .json settings file.");
            }

            var dataService = Activator.CreateInstance(dataServiceType) as IDataService;

            if (dataService == null)
            {
                throw new NullReferenceException($"Can't find create instance of type {nameof(dataServiceType)} from {settings.DataServiceFile} file. " +
                                                 "Please fix your .json settings file.");
            }

            dataService.Initialize(settings);
            screenshotService.InitializeDataService(dataService);
            
            var reporter = new Reporter
            {
                Action = new ActionHelper(settings.OutputPath),
                ScreenshotService = screenshotService,
                ReporterSettings = settings,
                ReportSettings = new ReportSettingsDto(settings.RunsToDisplay, settings.TestsToDisplay),
                DataService = dataService,
                RunRepository = new RunDtoRepository(),
                TestRunDtosRepository = new TestRunDtosRepository(),
                TestRunDtoProcessor = new TestRunDtoProcessor(),
                TestRunStarted = false
            };
            return reporter;
        }
    }
}