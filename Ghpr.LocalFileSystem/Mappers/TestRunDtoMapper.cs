using System.Linq;
using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class TestRunDtoMapper
    {
        public static TestRun Map(this TestRunDto testRunDto, SimpleItemInfoDto testOutputInfoDto)
        {
            var testRun = new TestRun
            {
                Categories = testRunDto.Categories,
                Description = testRunDto.Description,
                Events = testRunDto.Events.Select(teDto => new TestEvent
                {
                    Started = teDto.Started,
                    Finished = teDto.Finished,
                    Comment = teDto.Comment,
                    EventInfo = teDto.EventInfo.MapSimpleItemInfo()
                }).ToList(),
                FullName = testRunDto.FullName,
                Name = testRunDto.Name,
                Output = testOutputInfoDto.MapSimpleItemInfo(),
                Priority = testRunDto.Priority,
                Result = testRunDto.Result,
                RunGuid = testRunDto.RunGuid,
                Screenshots = testRunDto.Screenshots.Select(sDto => sDto.MapSimpleItemInfo()).ToList(),
                TestInfo = testRunDto.TestInfo.MapTestRunInfo(),
                Duration = testRunDto.Duration.Equals(0.0)
                    ? (testRunDto.TestInfo.Finish - testRunDto.TestInfo.Start).TotalSeconds
                    : testRunDto.Duration,
                TestMessage = testRunDto.TestMessage,
                TestStackTrace = testRunDto.TestStackTrace,
                TestType = testRunDto.TestType,
                TestData = testRunDto.TestData.Select(tdDto => new TestData
                {
                    Actual = tdDto.Actual,
                    Expected = tdDto.Expected,
                    Comment = tdDto.Comment,
                    TestDataInfo = tdDto.TestDataInfo.MapSimpleItemInfo()
                }).ToList()
            };
            return testRun;
        }

        public static TestRunDto ToDto(this TestRun testRun, SimpleItemInfo testOutputInfo)
        {
            var testRunDto = new TestRunDto
            {
                Categories = testRun.Categories,
                Description = testRun.Description,
                Events = testRun.Events.Select(teDto => new TestEventDto
                {
                    Started = teDto.Started,
                    Finished = teDto.Finished,
                    Comment = teDto.Comment,
                    EventInfo = teDto.EventInfo.ToDto()
                }).ToList(),
                FullName = testRun.FullName,
                Name = testRun.Name,
                Output = testOutputInfo.ToDto(),
                Priority = testRun.Priority,
                Result = testRun.Result,
                RunGuid = testRun.RunGuid,
                Screenshots = testRun.Screenshots.Select(s => s.ToDto()).ToList(),
                TestInfo = testRun.TestInfo.ToDto(),
                Duration = testRun.Duration.Equals(0.0)
                    ? (testRun.TestInfo.Finish - testRun.TestInfo.Start).TotalSeconds
                    : testRun.Duration,
                TestMessage = testRun.TestMessage,
                TestStackTrace = testRun.TestStackTrace,
                TestType = testRun.TestType,
                TestData = testRun.TestData.Select(tdDto => new TestDataDto
                {
                    Actual = tdDto.Actual,
                    Expected = tdDto.Expected,
                    Comment = tdDto.Comment,
                    TestDataInfo = tdDto.TestDataInfo.ToDto()
                }).ToList()
            };
            return testRunDto;
        }
    }
}