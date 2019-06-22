namespace Ghpr.Core.Core.Common
{
    public class RunSummaryDto
    {
        public int Total { get; set; }
        public int Success { get; set; }
        public int Errors { get; set; }
        public int Failures { get; set; }
        public int Inconclusive { get; set; }
        public int Ignored { get; set; }
        public int Unknown { get; set; }

        public RunSummaryDto(int total = 0, int success = 0, int errors = 0, int failures = 0, int inconclusive = 0, int ignored = 0, int unknown = 0)
        {
            Total = total;
            Success = success;
            Errors = errors;
            Failures = failures;
            Inconclusive = inconclusive;
            Ignored = ignored;
            Unknown = unknown;
        }
    }
}