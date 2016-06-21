namespace Ghpr.Core.Interfaces
{
    public interface IRunSummary
    {
        int Total { get; set; }
        int Success { get; set; }
        int Errors { get; set; }
        int Failures { get; set; }
        int Inconclusive { get; set; }
        int Ignored { get; set; }
        int Unknown { get; set; }
    }
}