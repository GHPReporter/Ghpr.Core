using System;
using System.Linq;
using Ghpr.Core.Common;

namespace Ghpr.Core.Extensions
{
    public static class TestRunExtensions
    {
        public static TestRunDto UpdateWithExistingTest(this TestRunDto target, TestRunDto run)
        {
            if (target.TestInfo.Guid.Equals(Guid.Empty))
            {
                target.TestInfo.Guid = target.FullName.ToMd5HashGuid();
            }
            target.Screenshots.AddRange(run.Screenshots.Where(s => !target.Screenshots.Any(ts => ts.Name.Equals(s.Name))));
            target.Events.AddRange(run.Events.Where(e => !target.Events.Any(te => te.Name.Equals(e.Name))));
            return target;
        }
    }
}