///<reference path="./../entities/TestRun.ts"/>
///<reference path="./../entities/TestEvent.ts"/>
///<reference path="./../entities/TestScreenshot.ts"/>
///<reference path="./../entities/TestData.ts"/>
///<reference path="./../../../dto/TestRunDto.ts"/>
///<reference path="./../../../dto/TestEventDto.ts"/>
///<reference path="./../../../dto/TestScreenshotDto.ts"/>
///<reference path="./../../../dto/TestDataDto.ts"/>
///<reference path="./ItemInfoDtoMapper.ts"/>

class TestRunDtoMapper {
    static map(testRun: TestRun): TestRunDto {
        let eventDtos = new Array<TestEventDto>(testRun.events.length);
        let screenshotDtos = new Array<TestScreenshotDto>(testRun.screenshots.length);
        let testDataDtos = new Array<TestDataDto>(testRun.testData.length);

        for (let i = 0; i < testRun.events.length; i++) {
            let event = testRun.events[i];
            let eventDto = new TestEventDto();
            eventDto.name = event.name;
            eventDto.started = event.started;
            eventDto.finished = event.finished;
            eventDtos[i] = eventDto;
        }

        for (let i = 0; i < testRun.screenshots.length; i++) {
            let screenshot = testRun.screenshots[i];
            let screenshotDto = new TestScreenshotDto();
            screenshotDto.date = screenshot.date;
            screenshotDto.data = screenshot.data;
            screenshotDto.testGuid = screenshot.testGuid;
            screenshotDtos[i] = screenshotDto;
        }

        for (let i = 0; i < testRun.events.length; i++) {
            let event = testRun.events[i];
            let eventDto = new TestEventDto();
            eventDto.name = event.name;
            eventDto.started = event.started;
            eventDto.finished = event.finished;
            eventDtos[i] = eventDto;
        }

        let testRunDto = new TestRunDto();
        testRunDto.name = testRun.name;
        testRunDto.categories = testRun.categories;
        testRunDto.description = testRunDto.description;
        testRunDto.duration = testRun.duration;
        testRunDto.events = eventDtos;
        testRunDto.fullName = testRun.fullName;
        testRunDto.output = testRun.output;
        testRunDto.priority = testRun.priority;
        testRunDto.result = testRun.result;
        testRunDto.testInfo = ItemInfoDtoMapper.map(testRun.testInfo);
        testRunDto.testMessage = testRun.testMessage;
        testRunDto.testStackTrace = testRun.testStackTrace;
        testRunDto.runGuid = testRun.runGuid;
        testRunDto.testType = testRun.testType;
        testRunDto.screenshots = screenshotDtos;
        testRunDto.testData = testDataDtos;
        
        return testRunDto;
    }
}