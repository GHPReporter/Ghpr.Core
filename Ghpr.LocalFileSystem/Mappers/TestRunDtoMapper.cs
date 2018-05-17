using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class TestRunDtoMapper
    {
        public static TestRun Map(this TestRunDto testRunDto)
        {
            var testRun = new TestRun();
            return testRun;
        }
    }
}