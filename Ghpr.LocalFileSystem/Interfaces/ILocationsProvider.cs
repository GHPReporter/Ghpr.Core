using System;
using Ghpr.Core.Utils;

namespace Ghpr.LocalFileSystem.Interfaces
{
    public interface ILocationsProvider
    {
        string SrcFolderPath { get; }
        string RunsFolderPath { get; }
        string TestsFolderPath { get; }
        string OutputPath { get; }
        Paths Paths { get; }

        string GetRunFullPath(Guid runGuid);
        string GetRunsFullPath();
        string GetTestFullPath();
        string GetTestOutputFullPath();
        string GetTestScreenshotFullPath();

        string GetTestFolderPath(Guid testGuid);
        string GetScreenshotFolderPath(string testGuid);
        string GetTestOutputFolderPath(Guid testGuid);
    }
}