using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class RunSummaryDtoMapper
    {
        public static RunSummary Map(this RunSummaryDto runSummaryDto)
        {
            var runSummary = new RunSummary
            {
                Total = runSummaryDto.Total,
                Errors = runSummaryDto.Errors,
                Ignored = runSummaryDto.Ignored,
                Failures = runSummaryDto.Failures,
                Inconclusive = runSummaryDto.Inconclusive,
                Success = runSummaryDto.Success,
                Unknown = runSummaryDto.Unknown
            };
            return runSummary;
        }
    }
}