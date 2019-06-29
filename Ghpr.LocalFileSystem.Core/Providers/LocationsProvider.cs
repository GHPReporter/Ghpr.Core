using System;
using System.IO;
using Ghpr.Core.Providers;
using Ghpr.Core.Utils;
using Ghpr.LocalFileSystem.Interfaces;

namespace Ghpr.LocalFileSystem.Providers
{
    public class LocationsProvider : ILocationsProvider
    {
        public LocationsProvider(string outputPath)
        {
            Paths = new Paths();
            OutputPath = outputPath;
            TestsFolderPath = Path.Combine(outputPath, Paths.Folder.Tests);
            RunsFolderPath = Path.Combine(outputPath, Paths.Folder.Runs);
            SrcFolderPath = Path.Combine(outputPath, Paths.Folder.Src);
        }

        public string SrcFolderPath { get; }
        public string TestsFolderPath { get; }
        public string RunsFolderPath { get; }
        public string OutputPath { get; }
        public Paths Paths { get; }

        public string GetTestFolderPath(Guid testGuid)
        {
            return Path.Combine(TestsFolderPath, testGuid.ToString());
        }

        public string GetTestOutputFolderPath(Guid testGuid)
        {
            return Path.Combine(TestsFolderPath, testGuid.ToString());
        }

        public string GetScreenshotFolderPath(Guid testGuid)
        {
            return Path.Combine(TestsFolderPath, testGuid.ToString(), Paths.Folder.Img);
        }

        public string GetReportSettingsFullPath()
        {
            return Path.Combine(SrcFolderPath, Paths.Files.ReportSettings);
        }

        public string GetRunFullPath(Guid runGuid)
        {
            return Path.Combine(RunsFolderPath, NamesProvider.GetRunFileName(runGuid));
        }

        public string GetRunsFullPath()
        {
            return Path.Combine(RunsFolderPath, Paths.File.Runs);
        }

        public string GetTestFullPath(Guid testGuid, DateTime testFinishDateTime)
        {
            return Path.Combine(GetTestFolderPath(testGuid), 
                NamesProvider.GetTestRunFileName(testFinishDateTime));
        }

        public string GetTestOutputFullPath(Guid testGuid, DateTime testFinishDateTime)
        {
            return Path.Combine(GetTestOutputFolderPath(testGuid),
                NamesProvider.GetTestOutputFileName(testFinishDateTime));
        }

        public string GetTestScreenshotFullPath(Guid testGuid, DateTime creationDateTime)
        {
            return Path.Combine(GetScreenshotFolderPath(testGuid),
                NamesProvider.GetScreenshotFileName(creationDateTime));
        }
    }
}