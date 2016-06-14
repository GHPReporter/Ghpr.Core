using System;
using Ghpr.Core.Common;

namespace Ghpr.Core.Extensions
{
    public static class TestRunExtensions
    {
        public static void Finished(this TestRun testRun)
        {
            testRun.DateTimeFinish = DateTime.Now;
        }
    }
}