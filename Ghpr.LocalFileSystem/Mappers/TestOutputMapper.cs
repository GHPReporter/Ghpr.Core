using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class TestOutputMapper
    {
        public static TestOutput Map(this TestOutputDto testOutputDto)
        {
            var name = LocationsProvider.GetTestOutputFileName(testOutputDto.TestOutputInfo.Date);
            var testOutput = new TestOutput
            {
                SuiteOutput = testOutputDto.SuiteOutput,
                Output = testOutputDto.Output,
                TestOutputInfo = testOutputDto.TestOutputInfo.MapSimpleItemInfo(name)
            };
            return testOutput;
        }
    }
}