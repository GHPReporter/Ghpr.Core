/// <reference path="./common/localFileSystem/LocalFileSystemDataService.ts" />
/// <reference path="./common/localFileSystem/LocalFileSystemPathsHelper.ts" />

class Controller {
    static reportSettings: ReportSettingsDto;
    static dataService: IDataService;

    static init(pageType: PageType, callback: (dataService: IDataService, reportSettings: ReportSettingsDto) => void): void {
        const settingsPath = LocalFileSystemPathsHelper.getReportSettingsPath(pageType);
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
                    const reportSettings: ReportSettingsDto = JSON.parse(req.responseText);
                    this.reportSettings = reportSettings;
                    this.dataService = new LocalFileSystemDataService();
                    this.dataService.reportSettings = reportSettings;
                    callback(this.dataService, this.reportSettings);
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