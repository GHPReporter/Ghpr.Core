///<reference path="./ITestScreenshot.ts"/>
///<reference path="./ITestEvent.ts"/>
///<reference path="./IItemInfo.ts"/>
///<reference path="./../enums/TestResult.ts"/>

interface ITestRun {
    name: string;
    fullName: string;
    duration: number;
    testInfo: IItemInfo;
    testStackTrace: string;
    testMessage: string;
    result: string;
    output: string;
    runGuid: string;
    testType: string;
    priority: string;
    categories: Array<string>;
    screenshots: Array<ITestScreenshot>;
    events: Array<ITestEvent>;

    testRunColor: string;
    testResult: TestResult;
}