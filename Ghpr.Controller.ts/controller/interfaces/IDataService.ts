///<reference path="./../dto/RunDto.ts"/>
///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../dto/ReportSettingsDto.ts"/>

interface IDataService {
    getRunDto(guid: string, start: Date, finish: Date): RunDto;
    getTestRunDto(guid: string, start: Date, finish: Date): TestRunDto;
    getReportSettingsDto() : ReportSettingsDto;
}