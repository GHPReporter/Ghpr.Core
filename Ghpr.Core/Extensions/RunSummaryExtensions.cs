using Ghpr.Core.Common;
using Ghpr.Core.Enums;

namespace Ghpr.Core.Extensions
{
    public static class RunSummaryExtensions
    {
        public static RunSummaryDto Update(this RunSummaryDto s, TestRunDto r)
        {
            switch (r.TestResult)
            {
                case TestResult.Passed:
                    s.Success++;
                    break;
                case TestResult.Failed:
                    s.Failures++;
                    break;
                case TestResult.Broken:
                    s.Errors++;
                    break;
                case TestResult.Ignored:
                    s.Ignored++;
                    break;
                case TestResult.Inconclusive:
                    s.Inconclusive++;
                    break;
                case TestResult.Unknown:
                    s.Unknown++;
                    break;
                default:
                    s.Unknown++;
                    break;
            }
            return s;
        }
    }
}