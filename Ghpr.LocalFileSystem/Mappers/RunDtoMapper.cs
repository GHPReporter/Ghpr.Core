using System.Linq;
using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class RunDtoMapper
    {
        public static Run Map(this RunDto runDto)
        {
            var run = new Run
            {
                TestRuns = runDto.TestsInfo.Select(
                    ti => ti.MapTestRunInfo()).ToList(),
                Name = runDto.Name,
                RunInfo = runDto.RunInfo.MapRunInfo(),
                RunSummary = runDto.RunSummary.Map(),
                Sprint = runDto.Sprint
            };
            return run;
        }
    }
}