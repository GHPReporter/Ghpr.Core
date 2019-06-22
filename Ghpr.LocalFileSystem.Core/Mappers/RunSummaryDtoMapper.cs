using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Core.Entities;

namespace Ghpr.LocalFileSystem.Core.Mappers
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

        public static RunSummaryDto ToDto(this RunSummary runSummary)
        {
            var runSummaryDto = new RunSummaryDto
            {
                Total = runSummary.Total,
                Errors = runSummary.Errors,
                Ignored = runSummary.Ignored,
                Failures = runSummary.Failures,
                Inconclusive = runSummary.Inconclusive,
                Success = runSummary.Success,
                Unknown = runSummary.Unknown
            };
            return runSummaryDto;
        }
    }
}