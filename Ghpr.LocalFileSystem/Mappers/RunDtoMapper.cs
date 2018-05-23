using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class RunDtoMapper
    {
        public static Run Map(this RunDto runDto)
        {
            var run = new Run
            {
                TestRunFiles = runDto.TestsInfo.Select(
                    ti => Path.Combine(ti.Guid.ToString(), LocationsProvider.GetTestRunFileName(ti.Finish))).ToList(),
                Name = runDto.Name,
                RunInfo = runDto.RunInfo.MapRunInfo(),
                RunSummary = runDto.RunSummary.Map(),
                Sprint = runDto.Sprint
            };
            return run;
        }

    }
}