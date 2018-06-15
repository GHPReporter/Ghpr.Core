///<reference path="./../dto/RunDto.ts"/>
///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../dto/ReportSettingsDto.ts"/>
///<reference path="./../enums/PageType.ts"/>

interface IDataService {
    reportSettings: ReportSettingsDto;

    fromPage(pageType: PageType): IDataService;

    getRun(runGuid: string, callback: Function): void;
    getLatestRuns(callback: Function): void;
    getLatestTest(testGuid: string, start: Date, finish: Date, callback: Function): void;
    getLatestTests(testGuid: string, count: number, callback: Function): void;
    getRunTests(runDto: RunDto, callback: (testRunDto: TestRunDto, c: number, i: number) => void): void;
}