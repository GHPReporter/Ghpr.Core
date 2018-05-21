using System;
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
        public static Reporter Build(IDataService dataService)
        {
            return InitializeReporter(ReporterSettingsProvider.Load(), dataService);
        }

        public static Reporter Build(ReporterSettings settings, IDataService dataService)
        {
            return InitializeReporter(settings, dataService);
        }

        public static Reporter Build(TestingFramework framework, IDataService dataService)
        {
            return InitializeReporter(ReporterSettingsProvider.Load(framework), dataService);

        }

        private static Reporter InitializeReporter(ReporterSettings settings, IDataService dataService)
        {
            if (settings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(settings.OutputPath),
                    "Reporter Output path must be specified. Please fix your .json settings file.");
            }

            Reporter.Action = new ActionHelper(settings.OutputPath);

            var reporter = new Reporter
            {
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