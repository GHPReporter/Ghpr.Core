/// <reference path="./common/localFileSystem/LocalFileSystemDataService.ts" />

class Controller {
    static reportSettings: ReportSettingsDto;
    static dataService: IDataService;

    static init(settingsPath: string,
        callback: (reportSettings: ReportSettingsDto, dataService: IDataService) => void): void {
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", settingsPath, true);
        req.onreadystatechange = () => {
            if (req.readyState === 4) {
                if (req.status !== 200 && req.status !== 0) {
                    console
                        .log(`Error while loading .json data: '${settingsPath}'! Status: ${req.status} : ${req
                            .statusText}`);
                } else {
                    this.reportSettings = JSON.parse(req.responseText);
                    this.dataService = new LocalFileSystemDataService();
                    callback(this.reportSettings, this.dataService);
                }
            }
        };
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(
                `Timeout while loading .json data: '${settingsPath}'! Status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
}