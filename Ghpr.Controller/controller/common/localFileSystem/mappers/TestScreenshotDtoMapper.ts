///<reference path="./../entities/TestScreenshot.ts"/>
///<reference path="./../../../dto/TestScreenshotDto.ts"/>
///<reference path="./../mappers/SimpleItemInfoDtoMapper.ts"/>

class TestScreenshotDtoMapper {
    static map(testScreenshot: TestScreenshot): TestScreenshotDto {
        let testScreenshotDto = new TestScreenshotDto();
        testScreenshotDto.base64Data = testScreenshot.base64Data;
        testScreenshotDto.format = testScreenshot.format;
        testScreenshotDto.testScreenshotInfo = SimpleItemInfoDtoMapper.map(testScreenshot.testScreenshotInfo);
        return testScreenshotDto;
    }
}