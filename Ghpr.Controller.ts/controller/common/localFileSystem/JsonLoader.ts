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
        this.loadJson(path, callback);
    }

    loadRunsJson(callback: Function): void {
        const path = LocalFileSystemPathsHelper.getRunsPath(this.pageType);
        this.loadJson(path, callback);
    }

    loadReportSettingsJson(callback: Function): void {
        const path = LocalFileSystemPathsHelper.getReportSettingsPath(this.pageType);
        this.loadJson(path, callback);
    }

    loadTestJson(testGuid: string, testFileName: string, callback: Function): void {
        const path = LocalFileSystemPathsHelper.getTestPath(testGuid, testFileName, this.pageType);
        this.loadJson(path, callback);
    }

    loadTestsJson(testGuid: string, callback: Function): void {
        const path = LocalFileSystemPathsHelper.getTestsPath(testGuid, this.pageType);
        this.loadJson(path, callback);
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

    loadAllJsons(paths: Array<string>, ind: number, resps: Array<string>, callback: Function): void {
        const count = paths.length;
        if (ind >= count) {
            callback(resps);
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
                    resps[ind] = req.responseText;
                    ind++;
                    this.loadAllJsons(paths, ind, resps, callback);
                }
        }
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }

    loadJsons(paths: Array<string>, ind: number, callback: Function): void {
        const count = paths.length;
        this.progressBar.reset(count);
        if (ind === 0) {
            this.progressBar.show();
        }
        if (ind >= count) {
            this.progressBar.hide();
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
                    callback(req.responseText, count, ind);
                    this.progressBar.onLoaded(ind);
                    ind++;
                    this.loadJsons(paths, ind, callback);
                }
        }
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
}