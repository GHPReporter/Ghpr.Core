///<reference path="./../../interfaces/IDataService.ts"/>
///<reference path="./../../dto/RunDto.ts/"/>
///<reference path="./../../dto/ReportSettingsDto.ts/"/>
///<reference path="./../../dto/TestRunDto.ts/"/>

class LocalFileSystemDataService implements IDataService{
    getRunDto(guid: string, start: Date, finish: Date): RunDto {
         throw new Error("Not implemented");
    }

    getTestRunDto(guid: string, start: Date, finish: Date): TestRunDto {
         throw new Error("Not implemented");
    }

    getReportSettingsDto(): ReportSettingsDto {
         throw new Error("Not implemented");
    }

    reportSettings: ReportSettingsDto;
}