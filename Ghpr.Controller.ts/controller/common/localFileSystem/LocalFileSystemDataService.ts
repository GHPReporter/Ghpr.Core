///<reference path="./../../interfaces/IDataService.ts"/>

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