using System;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Processors
{
    public class TestRunDtoProcessor : ITestRunDtoProcessor
    {
        public TestRunDto Process(TestRunDto testRunDtoWhenStarted, TestRunDto testRunDtoWhenFinished, Guid runGuid)
        {
            var finalTestRunDto = testRunDtoWhenFinished;
            if (finalTestRunDto.TestInfo.Guid.Equals(Guid.Empty))
            {
                finalTestRunDto.TestInfo.Guid = finalTestRunDto.FullName.ToMd5HashGuid();
            }
            finalTestRunDto.Screenshots
                .AddRange(testRunDtoWhenStarted.Screenshots.Where(
                    s => !finalTestRunDto.Screenshots.Any(ts => ts.Date.Equals(s.Date))));
            finalTestRunDto.Events.
                AddRange(testRunDtoWhenStarted.Events.Where(
                    e => !finalTestRunDto.Events.Any(te => te.Comment.Equals(e.Comment))));
            finalTestRunDto.TestInfo.Start = testRunDtoWhenStarted.TestInfo.Start.Equals(default(DateTime))
                ? finalTestRunDto.TestInfo.Start
                : testRunDtoWhenStarted.TestInfo.Start;
            finalTestRunDto.TestInfo.Finish = finalTestRunDto.TestInfo.Finish.Equals(default(DateTime))
                ? DateTime.Now
                : finalTestRunDto.TestInfo.Finish;
            finalTestRunDto.RunGuid = runGuid;
            return finalTestRunDto;
        }
    }
}