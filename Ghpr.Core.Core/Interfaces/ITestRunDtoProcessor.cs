using System;
using Ghpr.Core.Common;

namespace Ghpr.Core.Interfaces
{
    public interface ITestRunDtoProcessor
    {
        TestRunDto Process(TestRunDto testRunDtoWhenStated, TestRunDto testRunDtoWhenFinished, Guid runGuid);
    }
}