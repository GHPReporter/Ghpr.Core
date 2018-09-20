///<reference path="./../entities/TestOutput.ts"/>
///<reference path="./../../../dto/TestOutputDto.ts"/>
///<reference path="./../mappers/SimpleItemInfoDtoMapper.ts"/>

class TestOutputDtoMapper {
    static map(testOutput: TestOutput): TestOutputDto {
        let testOutputDto = new TestOutputDto();
        testOutputDto.output = testOutput.output;
        testOutputDto.suiteOutput = testOutput.suiteOutput;
        testOutputDto.testOutputInfo = SimpleItemInfoDtoMapper.map(testOutput.testOutputInfo);
        return testOutputDto;
    }
}