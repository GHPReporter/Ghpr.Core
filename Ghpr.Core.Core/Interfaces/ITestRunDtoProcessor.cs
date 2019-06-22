using System;
using Ghpr.Core.Core.Common;

namespace Ghpr.Core.Core.Interfaces
{
    public interface ITestRunDtoProcessor
    {
        TestRunDto Process(TestRunDto testRunDtoWhenStated, TestRunDto testRunDtoWhenFinished, Guid runGuid);
    }
}