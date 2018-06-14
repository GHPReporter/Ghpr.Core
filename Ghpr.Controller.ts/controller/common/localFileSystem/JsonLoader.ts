///<reference path="./entities/Run.ts"/>
///<reference path="./../../enums/PageType.ts"/>
///<reference path="./LocalFileSystemPathsHelper.ts"/>
///<reference path="./../ProgressBar.ts"/>

class JsonLoader {

    private pageType: PageType;
    private progressBar: ProgressBar;

    constructor(pt: PageType) {
        this.pageType = pt;
        this.progressBar = new ProgressBar(1);
    }

    loadRunJson(runGuid: string, callback: Function): void {
        const path = LocalFileSystemPathsHelper.getRunPath(this.pageType, runGuid);
        this.loadJsonsByPaths([path], 0, new Array(), false, true, callback);
    }

    loadRunsJson(callback: Function): void {
        const path = LocalFileSystemPathsHelper.getRunsPath(this.pageType);
        this.loadJsonsByPaths([path], 0, new Array(), false, true, callback);
    }

    loadReportSettingsJson(callback: Function): void {
        const path = LocalFileSystemPathsHelper.getReportSettingsPath(this.pageType);
        this.loadJsonsByPaths([path], 0, new Array(), false, true, callback);
    }

    loadTestJson(testGuid: string, testFileName: string, callback: Function): void {
        const path = LocalFileSystemPathsHelper.getTestPath(testGuid, testFileName, this.pageType);
        this.loadJsonsByPaths([path], 0, new Array(), false, true, callback);
    }

    loadTestsJson(testGuid: string, callback: Function): void {
        const path = LocalFileSystemPathsHelper.getTestsPath(testGuid, this.pageType);
        this.loadJsonsByPaths([path], 0, new Array(), false, true, callback);
    }

    loadJsonsByPaths(paths: Array<string>, ind: number, responses: Array<string>, showProgressBar: boolean, callbackForEach: boolean, callback: Function): void {
        const count = paths.length;
        if (showProgressBar) {
            this.progressBar.reset(count);
            if (ind === 0) {
                this.progressBar.show();
            }
            if (ind >= count) {
                this.progressBar.hide();
                return;
            }
        }
        if (!callbackForEach && ind >= count) {
            callback(responses, count, ind);
        }
        if (ind >= count) {
            return;
        }
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", paths[ind], true);
        req.onreadystatechange = () => {
            if (req.readyState === 4)
                if (req.status !== 200 && req.status !== 0) {
                    console
                        .log(`Error while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
                } else {
                    responses[ind] = req.responseText;
                    if (callbackForEach) {
                        callback(req.responseText, count, ind);
                    }
                    if (showProgressBar) {
                        this.progressBar.onLoaded(ind);
                    }
                    ind++;
                    this.loadJsonsByPaths(paths, ind, responses, showProgressBar, callbackForEach, callback);
                }
        }
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
}