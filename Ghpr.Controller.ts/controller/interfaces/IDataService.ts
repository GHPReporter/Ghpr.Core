///<reference path="./../dto/RunDto.ts"/>
///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../dto/ReportSettingsDto.ts"/>
///<reference path="./../enums/PageType.ts"/>

interface IDataService {
    reportSettings: ReportSettingsDto;

    fromPage(pageType: PageType): IDataService;

    getRun(runGuid: string, callback: Function): void;
    getRunInfos(callback: (runInfoDtos: ItemInfoDto[]) => void): void;
    getLatestRuns(callback: Function): void;
    getLatestTest(testGuid: string, finish: Date, callback: Function): void;
    getTestInfos(testGuid: string, callback: (testInfoDtos: ItemInfoDto[]) => void): void;
    getLatestTests(testGuid: string, callback: Function): void;
    getRunTests(runDto: RunDto, callback: (testRunDto: TestRunDto, c: number, i: number) => void): void;
}