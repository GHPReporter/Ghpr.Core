///<reference path="./../dto/RunDto.ts"/>
///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../dto/ReportSettingsDto.ts"/>

interface IDataService {

    reportSettings: ReportSettingsDto;

    getRunDto(guid: string, start: Date, finish: Date, callback: Function): RunDto;
    getTestRunDto(guid: string, start: Date, finish: Date, callback: Function): TestRunDto;
    getReportSettingsDto(callback: Function) : ReportSettingsDto;
}