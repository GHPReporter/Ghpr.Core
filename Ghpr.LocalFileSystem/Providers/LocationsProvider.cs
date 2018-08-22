using System;
using System.IO;
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
            TestsPath = Path.Combine(outputPath, Paths.Folder.Tests);
            RunsPath = Path.Combine(outputPath, Paths.Folder.Runs);
            SrcPath = Path.Combine(outputPath, Paths.Folder.Src);
        }

        public string SrcPath { get; }
        public string TestsPath { get; }
        public string RunsPath { get; }
        public string OutputPath { get; }
        public Paths Paths { get; }

        public string GetTestPath(Guid testGuid)
        {
            return Path.Combine(TestsPath, testGuid.ToString());
        }

        public string GetRunFullPath(Guid runGuid)
        {
            return Path.Combine(RunsPath, NamesProvider.GetRunFileName(runGuid));
        }

        public string GetRelativeTestRunPath(string testGuid, string testFileName)
        {
            return $"{testGuid}\\{testFileName}";
        }

        public string GetScreenshotPath(string testGuid)
        {
            return Path.Combine(TestsPath, testGuid, Paths.Folder.Img);
        }
    }
}