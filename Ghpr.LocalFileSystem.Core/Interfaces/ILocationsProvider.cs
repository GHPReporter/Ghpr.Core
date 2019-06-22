using System;
using Ghpr.Core.Core.Utils;

namespace Ghpr.LocalFileSystem.Core.Interfaces
{
    public interface ILocationsProvider
    {
        string SrcFolderPath { get; }
        string RunsFolderPath { get; }
        string TestsFolderPath { get; }
        string OutputPath { get; }
        Paths Paths { get; }

        string GetReportSettingsFullPath();
        string GetRunFullPath(Guid runGuid);
        string GetRunsFullPath();
        string GetTestFullPath(Guid testGuid, DateTime testFinishDateTime);
        string GetTestOutputFullPath(Guid testGuid, DateTime testFinishDateTime);
        string GetTestScreenshotFullPath(Guid testGuid, DateTime creationDateTime);

        string GetTestFolderPath(Guid testGuid);
        string GetScreenshotFolderPath(Guid testGuid);
        string GetTestOutputFolderPath(Guid testGuid);
    }
}