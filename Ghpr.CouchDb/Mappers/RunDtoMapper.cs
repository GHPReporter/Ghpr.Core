using System.Linq;
using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class RunDtoMapper
    {
        public static DatabaseEntity<Run> Map(this RunDto runDto)
        {
            var run = new Run
            {
                Name = runDto.Name,
                RunInfo = runDto.RunInfo.MapRunInfo(),
                RunSummary = runDto.RunSummary.Map(),
                Sprint = runDto.Sprint,
                TestRuns = runDto.TestsInfo.Select(ti => new ItemInfo
                {
                    Guid = ti.Guid,
                    Start = ti.Start,
                    Finish = ti.Finish
                }).ToList()
            };
            var entity = new DatabaseEntity<Run>
            {
                Data = run,
                Id = $"run_{run.RunInfo.Guid}",
                Rev = $"1-{run.RunInfo.Start:yyyyMMdd_HHmmssfff}",
                Type = EntityType.RunType
            };
            return entity;
        }
    }
}