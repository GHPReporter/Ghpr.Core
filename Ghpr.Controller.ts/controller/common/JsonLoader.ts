﻿///<reference path="./../interfaces/IRun.ts"/>
///<reference path="./../enums/PageType.ts"/>
///<reference path="./PathsHelper.ts"/>
///<reference path="./ProgressBar.ts"/>

class JsonLoader {

    private pageType: PageType;

    constructor(pt: PageType) {
        this.pageType = pt;
    }

    loadRunJson(runGuid: string, callback: Function): void {
        const path = PathsHelper.getRunPath(this.pageType, runGuid);
        this.loadJson(path, callback);
    }

    loadRunsJson(callback: Function): void {
        const path = PathsHelper.getRunsPath(this.pageType);
        this.loadJson(path, callback);
    }

    loadReportSettingsJson(callback: Function): void {
        const path = PathsHelper.getReportSettingsPath(this.pageType);
        this.loadJson(path, callback);
    }

    loadTestJson(testGuid: string, testFileName: string, callback: Function): void {
        const path = PathsHelper.getTestPath(testGuid, testFileName, this.pageType);
        this.loadJson(path, callback);
    }

    loadTestsJson(testGuid: string, callback: Function): void {
        const path = PathsHelper.getTestsPath(testGuid, this.pageType);
        this.loadJson(path, callback);
    }

    loadJson(path: string, callback: Function): void {
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", path, true);
        req.onreadystatechange = () => {
            if (req.readyState === 4)
                if (req.status !== 200) {
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

    loadAllJsons(paths: Array<string>, ind: number, resps: Array<string>, callback: Function,
        loadAll: boolean = true): void {
        const count = paths.length;
        if (loadAll && ind >= count) {
            callback(resps);
            return;
        }
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", paths[ind], true);
        req.onreadystatechange = () => {
            if (req.readyState === 4)
                if (req.status !== 200) {
                    console
                        .log(`Error while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
                } else {
                    resps[ind] = req.responseText;
                    ind++;
                    this.loadAllJsons(paths, ind, resps, callback, loadAll);
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
        const pb = new ProgressBar(paths.length);
        if (ind === 0) {
            pb.show();
        }
        if (ind >= count) {
            pb.hide();
            return;
        }
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", paths[ind], true);
        req.onreadystatechange = () => {
            if (req.readyState === 4)
                if (req.status !== 200) {
                    console
                        .log(`Error while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
                } else {
                    callback(req.responseText, count, ind);
                    pb.onLoaded(ind);
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

    static reviveRun(key: any, value: any): any {
        if (key === "start" || key === "finish" || key === "date") return new Date(value);
        //if (key === "duration") return new Number(value);
        return value;
    }
}