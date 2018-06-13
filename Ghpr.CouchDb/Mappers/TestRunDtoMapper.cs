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
            var testRun = new TestRun
            {
                Categories = testRunDto.Categories,
                Description = testRunDto.Description,
                Events = testRunDto.Events.Select(teDto => new TestEvent
                {
                    Started = teDto.Started,
                    Finished = teDto.Finished,
                    Name = teDto.Name
                }).ToList(),
                FullName = testRunDto.FullName,
                Name = testRunDto.Name,
                Output = testRunDto.Output,
                Priority = testRunDto.Priority,
                Result = testRunDto.Result,
                RunGuid = testRunDto.RunGuid,
                Screenshots = new List<TestScreenshotInfo>(),
                TestInfo = testRunDto.TestInfo.MapTestRunInfo(),
                TestDuration = (testRunDto.TestInfo.Finish - testRunDto.TestInfo.Start).TotalSeconds,
                TestMessage = testRunDto.TestMessage,
                TestStackTrace = testRunDto.TestStackTrace,
                TestType = testRunDto.TestType,
                TestData = testRunDto.TestData.Select(tdDto => new TestData
                {
                    Actual = tdDto.Actual,
                    Expected = tdDto.Expected,
                    Comment = tdDto.Comment,
                    Date = tdDto.Date
                }).ToList()
            };
            var entity = new DatabaseEntity<TestRun>
            {
                Data = testRun,
                Id = $"test_run_{testRun.TestInfo.Guid}-{testRun.TestInfo.Start:yyyyMMdd_HHmmssfff}-{testRun.TestInfo.Finish:yyyyMMdd_HHmmssfff}",
                Rev = $"1-{testRun.TestInfo.Start:yyyyMMdd_HHmmssfff}-{testRun.TestInfo.Finish:yyyyMMdd_HHmmssfff}",
                Type = EntityType.TestRunType
            };
            return entity;
        }
    }
}