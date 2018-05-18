using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Common;

namespace Ghpr.Core.Extensions
{
    public static class TestRunDtoListExtensions
    {
        public static DateTime GetRunStartDateTime(this List<TestRunDto> testRuns)
        {
            var runStart = testRuns.OrderBy(t => t.TestInfo.Start)
                               .FirstOrDefault(t => !t.TestInfo.Start.Equals(default(DateTime)))?.TestInfo?.Start 
                               ?? default(DateTime);
            return runStart;
        }

        public static DateTime GetRunFinishDateTime(this List<TestRunDto> testRuns)
        {
            var runFinish = testRuns.OrderByDescending(t => t.TestInfo.Finish)
                                .FirstOrDefault(t => !t.TestInfo.Start.Equals(default(DateTime)))?.TestInfo?.Finish 
                                ?? default(DateTime);
            return runFinish;
        }
    }
}