using System;
using System.Collections.Generic;
using System.Linq;
using Ghpr.Core.Core.Common;

namespace Ghpr.Core.Core.Extensions
{
    public static class TestRunDtoListExtensions
    {
        public static DateTime GetRunStartDateTime(this List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns)
        {
            var runStart = testRuns.OrderBy(t => t.Key.TestInfo.Start)
                               .FirstOrDefault(t => !t.Key.TestInfo.Start.Equals(default(DateTime))).Key.TestInfo?.Start 
                               ?? default(DateTime);
            return runStart;
        }

        public static DateTime GetRunFinishDateTime(this List<KeyValuePair<TestRunDto, TestOutputDto>> testRuns)
        {
            var runFinish = testRuns.OrderByDescending(t => t.Key.TestInfo.Finish)
                                .FirstOrDefault(t => !t.Key.TestInfo.Start.Equals(default(DateTime))).Key.TestInfo?.Finish 
                                ?? default(DateTime);
            return runFinish;
        }
    }
}