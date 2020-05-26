///<reference path="./TestScreenshot.ts"/>
///<reference path="./TestData.ts"/>
///<reference path="./TestEvent.ts"/>
///<reference path="./ItemInfo.ts"/>
///<reference path="./../../../enums/TestResult.ts"/>

class TestRun {
    name: string;
    fullName: string;
    description: string;
    testInfo: ItemInfo;
    testStackTrace: string;
    testMessage: string;
    result: string;
    output: SimpleItemInfo;
    runGuid: string;
    testType: string;
    priority: string;
    categories: Array<string>;
    screenshots: Array<SimpleItemInfo>;
    testData: Array<TestData>;
    events: Array<TestEvent>;

    testRunColor: string;
    testResult: TestResult;
}