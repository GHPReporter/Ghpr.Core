using System.IO;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Helpers
{
    public class LocationsProvider : ILocationsProvider
    {
        public LocationsProvider(IReporterSettings reporterSettings)
        {
            ReporterSettings = reporterSettings;
            TestsPath = Path.Combine(reporterSettings.OutputPath, Paths.Folders.Tests);
            RunsPath = Path.Combine(reporterSettings.OutputPath, Paths.Folders.Runs);
        }

        public string TestsPath { get; }
        public string RunsPath { get; }
        public IReporterSettings ReporterSettings { get; }

        public string GetTestPath(string testGuid)
        {
            return Path.Combine(TestsPath, testGuid);
        }

        public string GetRelativeTestRunPath(string testGuid, string testFileName)
        {
            return $"{testGuid}\\{testFileName}";
        }

        public string GetScreenshotPath(string testGuid)
        {
            return Path.Combine(TestsPath, testGuid, Paths.Folders.Img);
        }
    }
}