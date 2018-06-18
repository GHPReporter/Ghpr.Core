using System.Linq;
using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class RunDtoMapper
    {
        public static DatabaseEntity<Run> Map(this RunDto runDto)
        {
            var id = $"run_{runDto.RunInfo.Guid}";
            var run = new Run
            {
                Name = runDto.Name,
                RunInfo = runDto.RunInfo.MapRunInfo(id),
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
                Id = id,
                Type = EntityType.RunType
            };
            return entity;
        }
    }
}