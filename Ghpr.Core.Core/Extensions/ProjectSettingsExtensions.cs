using Ghpr.Core.Settings;

namespace Ghpr.Core.Extensions
{
    public static class ProjectSettingsExtensions
    {
        public static ProjectSettings GetFromSourceOrDefault(this ProjectSettings source, ProjectSettings defaultSettings)
        {
            var ps = new ProjectSettings
            {
                OutputPath = source.OutputPath.ValueOrDefault(defaultSettings.OutputPath),
                DataServiceFile = source.DataServiceFile.ValueOrDefault(defaultSettings.DataServiceFile),
                LoggerFile = source.LoggerFile.ValueOrDefault(defaultSettings.LoggerFile),
                Sprint = source.Sprint.ValueOrDefault(defaultSettings.Sprint),
                ReportName = source.ReportName.ValueOrDefault(defaultSettings.ReportName),
                ProjectName = source.ProjectName.ValueOrDefault(defaultSettings.ProjectName),
                RunName = source.RunName.ValueOrDefault(defaultSettings.RunName),
                RunGuid = source.RunGuid.ValueOrDefault(defaultSettings.RunGuid),
                RealTimeGeneration = !source.RealTimeGeneration
                    ? defaultSettings.RealTimeGeneration
                    : source.RealTimeGeneration,
                RunsToDisplay = source.RunsToDisplay == 0 ? defaultSettings.RunsToDisplay : source.RunsToDisplay,
                TestsToDisplay = source.TestsToDisplay == 0 ? defaultSettings.TestsToDisplay : source.TestsToDisplay,
                Retention = source.Retention ?? defaultSettings.Retention,
                EscapeTestOutput = source.EscapeTestOutput
            };
            return ps;
        }
    }
}