using System.Linq;
using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class TestRunDtoMapper
    {
        public static TestRun Map(this TestRunDto testRunDto)
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
                Screenshots = testRunDto.Screenshots.Select(sDto => new TestScreenshot
                {
                    Date = sDto.Date
                }).ToList(),
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
            return testRun;
        }
    }
}