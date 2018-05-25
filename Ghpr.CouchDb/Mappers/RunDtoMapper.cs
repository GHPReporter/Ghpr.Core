using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class RunDtoMapper
    {
        public static Run Map(this RunDto runDto)
        {
            var run = new Run
            {
                Name = runDto.Name,
                RunInfo = runDto.RunInfo.MapRunInfo(),
                RunSummary = runDto.RunSummary.Map(),
                Sprint = runDto.Sprint
            };
            return run;
        }
    }
}