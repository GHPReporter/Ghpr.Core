///<reference path="./../dto/RunDto.ts"/>
///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../dto/ReportSettingsDto.ts"/>
///<reference path="./../enums/PageType.ts"/>

interface IDataService {
    reportSettings: ReportSettingsDto;

    fromPage(pageType: PageType): IDataService;

    getRun(runGuid: string, callback: (runDto: RunDto) => void): void;
    getRunInfos(callback: (runInfoDtos: ItemInfoDto[]) => void): void;
    getLatestRuns(callback: Function): void;
    getLatestTest(testGuid: string, itemName: string, callback: Function): void;
    getTestOutput(t: TestRunDto, callback: Function): void;
    getTestScreenshots(testRunDto: TestRunDto, callback: (screenshotDtos: Array<TestScreenshotDto>) => void): void;
    getTestInfos(testGuid: string, callback: (testInfoDtos: ItemInfoDto[]) => void): void;
    getLatestTests(testGuid: string, callback: Function): void;
    getRunTests(runDto: RunDto, callback: (testRunDto: TestRunDto, c: number, i: number) => void): void;
}