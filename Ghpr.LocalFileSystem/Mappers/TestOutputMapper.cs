using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class TestOutputMapper
    {
        public static TestOutput Map(this TestOutputDto testOutputDto)
        {
            var name = NamesProvider.GetTestOutputFileName(testOutputDto.TestOutputInfo.Date);
            var testOutput = new TestOutput
            {
                SuiteOutput = testOutputDto.SuiteOutput,
                Output = testOutputDto.Output,
                TestOutputInfo = testOutputDto.TestOutputInfo.MapSimpleItemInfo(name)
            };
            return testOutput;
        }

        public static TestOutputDto ToDto(this TestOutput testOutput)
        {
            var testOutputDto = new TestOutputDto
            {
                SuiteOutput = testOutput.SuiteOutput,
                Output = testOutput.Output,
                TestOutputInfo = testOutput.TestOutputInfo.ToDto()
            };
            return testOutputDto;
        }
    }
}