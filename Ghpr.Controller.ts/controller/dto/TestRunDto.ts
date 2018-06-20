///<reference path="./TestScreenshotDto.ts"/>
///<reference path="./TestDataDto.ts"/>
///<reference path="./TestEventDto.ts"/>
///<reference path="./ItemInfoDto.ts"/>
///<reference path="./SimpleItemInfoDto.ts"/>
///<reference path="./../enums/TestResult.ts"/>

class TestRunDto {
    name: string;
    fullName: string;
    description: string;
    duration: number;
    testInfo: ItemInfoDto;
    testStackTrace: string;
    testMessage: string;
    result: string;
    output: SimpleItemInfoDto;
    runGuid: string;
    testType: string;
    priority: string;
    categories: Array<string>;
    screenshots: Array<SimpleItemInfoDto>;
    testData: Array<TestDataDto>;
    events: Array<TestEventDto>;

    testRunColor: string;
    testResult: TestResult;
}