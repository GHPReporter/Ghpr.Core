using Ghpr.Core.Utils;

namespace Ghpr.LocalFileSystem.Interfaces
{
    public interface ILocationsProvider
    {
        string SrcPath { get; }
        string RunsPath { get; }
        string TestsPath { get; }
        string OutputPath { get; }
        Paths Paths { get; }

        string GetTestPath(string testGuid);
        string GetRelativeTestRunPath(string testGuid, string testFileName);
        string GetScreenshotPath(string testGuid);
    }
}