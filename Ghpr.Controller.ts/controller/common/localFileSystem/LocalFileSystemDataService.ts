///<reference path="./../../interfaces/IDataService.ts"/>
///<reference path="./../../dto/RunDto.ts"/>
///<reference path="./../../dto/ReportSettingsDto.ts"/>
///<reference path="./../../dto/TestRunDto.ts"/>
///<reference path="./../../enums/PageType.ts"/>

class LocalFileSystemDataService implements IDataService {
    private currentPage: PageType;

    fromPage(pageType: PageType): IDataService {
        this.currentPage = pageType;
        return this;
    }

    getRunDto(guid: string, start: Date, finish: Date, callback: Function): RunDto {
        throw new Error("Not implemented");
    }

    getTestRunDto(guid: string, start: Date, finish: Date, callback: Function): TestRunDto {
         throw new Error("Not implemented");
    }

    getReportSettingsDto(callback: Function): ReportSettingsDto {
         throw new Error("Not implemented");
    }

    loadJson(path: string, callback: Function): void {
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", path, true);
        req.onreadystatechange = () => {
            if (req.readyState === 4)
                if (req.status !== 200 && req.status !== 0) {
                    console
                        .log(`Error while loading .json data: '${path}'! Request status: ${req.status} : ${req.statusText}`);
                } else {
                    callback(req.responseText);
                }
        }
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${path}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
    
    reportSettings: ReportSettingsDto;
}