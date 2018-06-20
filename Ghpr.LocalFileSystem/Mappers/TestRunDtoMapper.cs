using System.Linq;
using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Mappers
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
                    Comment = teDto.Comment,
                    EventInfo = teDto.EventInfo.MapSimpleItemInfo()
                }).ToList(),
                FullName = testRunDto.FullName,
                Name = testRunDto.Name,
                Output = testRunDto.Output.MapSimpleItemInfo(LocationsProvider.GetTestOutputFileName(testRunDto.Output.Date)),
                Priority = testRunDto.Priority,
                Result = testRunDto.Result,
                RunGuid = testRunDto.RunGuid,
                Screenshots = testRunDto.Screenshots.Select(sDto => sDto.MapSimpleItemInfo()).ToList(),
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
                    TestDataInfo = tdDto.TestDataInfo.MapSimpleItemInfo()
                }).ToList()
            };
            return testRun;
        }
    }
}