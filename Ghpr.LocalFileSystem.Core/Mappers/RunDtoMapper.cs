using System.Linq;
using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Core.Entities;

namespace Ghpr.LocalFileSystem.Core.Mappers
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

        public static RunDto ToDto(this Run run)
        {
            var runDto = new RunDto
            {
                TestsInfo = run.TestRuns.Select(
                    tr => tr.ToDto()).ToList(),
                Name = run.Name,
                RunInfo = run.RunInfo.ToDto(),
                RunSummary = run.RunSummary.ToDto(),
                Sprint = run.Sprint
            };
            return runDto;
        }
    }
}