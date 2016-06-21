using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Extensions
{
    public static class RunSummaryExtensions
    {
        public static IRunSummary Update(this IRunSummary s, ITestRun r)
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