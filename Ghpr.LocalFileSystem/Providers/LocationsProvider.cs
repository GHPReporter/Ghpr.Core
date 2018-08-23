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

        public string GetScreenshotFolderPath(string testGuid)
        {
            return Path.Combine(TestsFolderPath, testGuid, Paths.Folder.Img);
        }


        public string GetRunFullPath(Guid runGuid)
        {
            return Path.Combine(RunsFolderPath, NamesProvider.GetRunFileName(runGuid));
        }

        public string GetRunsFullPath()
        {
            throw new NotImplementedException();
        }

        public string GetTestFullPath()
        {
            throw new NotImplementedException();
        }

        public string GetTestOutputFullPath()
        {
            throw new NotImplementedException();
        }

        public string GetTestScreenshotFullPath()
        {
            throw new NotImplementedException();
        }
    }
}