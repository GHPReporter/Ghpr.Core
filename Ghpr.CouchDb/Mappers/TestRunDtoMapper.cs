using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class TestRunDtoMapper
    {
        public static DatabaseEntity<TestRun> Map(this TestRunDto testRunDto)
        {
            var id = $"test_run_{testRunDto.TestInfo.Guid}" +
                     $"-{testRunDto.TestInfo.Start:yyyyMMdd_HHmmssfff}" +
                     $"-{testRunDto.TestInfo.Finish:yyyyMMdd_HHmmssfff}";
            var testRun = new TestRun
            {
                Categories = testRunDto.Categories,
                Description = testRunDto.Description,
                Events = testRunDto.Events.Select(teDto => new TestEvent
                {
                    Started = teDto.Started,
                    Finished = teDto.Finished,
                    Name = teDto.Comment,
                    EventInfo = teDto.EventInfo.MapSimpleItemInfo()
                }).ToList(),
                FullName = testRunDto.FullName,
                Name = testRunDto.Name,
                //TODO: Insert correct itemName here
                Output = testRunDto.Output.MapSimpleItemInfo(""),
                Priority = testRunDto.Priority,
                Result = testRunDto.Result,
                RunGuid = testRunDto.RunGuid,
                Screenshots = new List<TestScreenshotInfo>(),
                TestInfo = testRunDto.TestInfo.MapTestRunInfo(id),
                TestDuration = (testRunDto.TestInfo.Finish - testRunDto.TestInfo.Start).TotalSeconds,
                TestMessage = testRunDto.TestMessage,
                TestStackTrace = testRunDto.TestStackTrace,
                TestType = testRunDto.TestType,
                Duration = testRunDto.Duration,
                TestData = testRunDto.TestData.Select(tdDto => new TestData
                {
                    Actual = tdDto.Actual,
                    Expected = tdDto.Expected,
                    Comment = tdDto.Comment,
                    Date = tdDto.TestDataInfo.Date
                }).ToList()
            };
            var entity = new DatabaseEntity<TestRun>
            {
                Data = testRun,
                Id = id,
                Rev = $"1-{testRun.TestInfo.Start:yyyyMMdd_HHmmssfff}-{testRun.TestInfo.Finish:yyyyMMdd_HHmmssfff}",
                Type = EntityType.TestRunType
            };
            return entity;
        }
    }
}