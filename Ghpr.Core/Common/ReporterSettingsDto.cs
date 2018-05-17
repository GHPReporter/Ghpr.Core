namespace Ghpr.Core.Common
{
    public class ReporterSettingsDto
    {
        public string OutputPath { get; set; }
        public string Sprint { get; set; }
        public string RunName { get; set; }
        public string RunGuid { get; set; }
        public bool RealTimeGeneration { get; set; }
        public int RunsToDisplay { get; set; }
        public int TestsToDisplay { get; set; }
    }
}