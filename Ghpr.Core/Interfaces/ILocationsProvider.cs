﻿namespace Ghpr.Core.Interfaces
{
    public interface ILocationsProvider
    {
        string RunsPath { get; }
        string TestsPath { get; }
        IReporterSettings ReporterSettings { get; }
        string GetTestPath(string testGuid);
    }
}