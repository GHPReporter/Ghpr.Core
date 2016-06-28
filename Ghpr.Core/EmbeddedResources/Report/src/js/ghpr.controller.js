var TestResult;
(function (TestResult) {
    TestResult[TestResult["Passed"] = 0] = "Passed";
    TestResult[TestResult["Failed"] = 1] = "Failed";
    TestResult[TestResult["Broken"] = 2] = "Broken";
    TestResult[TestResult["Ignored"] = 3] = "Ignored";
    TestResult[TestResult["Inconclusive"] = 4] = "Inconclusive";
    TestResult[TestResult["Unknown"] = 5] = "Unknown";
})(TestResult || (TestResult = {}));
var PageType;
(function (PageType) {
    PageType[PageType["TestRunsPage"] = 0] = "TestRunsPage";
    PageType[PageType["TestRunPage"] = 1] = "TestRunPage";
    PageType[PageType["TestPage"] = 2] = "TestPage";
})(PageType || (PageType = {}));
class UrlHelper {
    static insertParam(key, value) {
        const paramsPart = document.location.search.substr(1);
        window.history.pushState("", "", "");
        const p = `${key}=${value}`;
        if (paramsPart === "") {
            window.history.pushState("", "", `?${p}`);
        }
        else {
            let params = paramsPart.split("&");
            const paramToChange = params.find((par) => par.split("=")[0] === key);
            if (paramToChange != undefined) {
                if (params.length === 1) {
                    params = [p];
                }
                else {
                    const index = params.indexOf(paramToChange);
                    params.splice(index, 1);
                    params.push(p);
                }
            }
            window.history.pushState("", "", `?${params.join("&")}`);
        }
    }
    static getParam(key) {
        const paramsPart = document.location.search.substr(1);
        if (paramsPart === "") {
            return "";
        }
        else {
            const params = paramsPart.split("&");
            const paramToGet = params.find((par) => par.split("=")[0] === key);
            if (paramToGet != undefined) {
                return paramToGet.split("=")[1];
            }
            else
                return "";
        }
    }
}
class DateFormatter {
    static format(date) {
        const year = `${date.getFullYear()}`;
        const month = this.correct(`${date.getMonth() + 1}`);
        const day = this.correct(`${date.getDate()}`);
        const hour = this.correct(`${date.getHours()}`);
        const minute = this.correct(`${date.getMinutes()}`);
        const second = this.correct(`${date.getSeconds()}`);
        return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
    }
    static diff(start, finish) {
        const timeDifference = (finish.getTime() - start.getTime());
        const dDate = new Date(timeDifference);
        const dHours = dDate.getUTCHours();
        const dMins = dDate.getUTCMinutes();
        const dSecs = dDate.getUTCSeconds();
        const dMilliSecs = dDate.getUTCMilliseconds();
        const readableDifference = dHours + ":" + dMins + ":" + dSecs + "." + dMilliSecs;
        return readableDifference;
    }
    static correct(s) {
        if (s.length === 1) {
            return `0${s}`;
        }
        else
            return s;
    }
}
class Color {
}
Color.passed = "#8bc34a";
Color.broken = "#ffc107";
Color.failed = "#ef5350";
Color.ignored = "#81d4fa";
Color.inconclusive = "#D6FAF7";
Color.unknown = "#bdbdbd";
class RunPageUpdater {
    static updateTime(run) {
        document.getElementById("start").innerHTML = `Start datetime: ${DateFormatter.format(run.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `Finish datetime: ${DateFormatter.format(run.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `Duration: ${DateFormatter.diff(run.runInfo.start, run.runInfo.finish)}`;
    }
    static updateName(run) {
        document.getElementById("run-name").innerHTML = run.name;
    }
    static updateSummary(run) {
        const s = run.summary;
        document.getElementById("total").innerHTML = `Total: ${s.total}`;
        document.getElementById("passed").innerHTML = `Success: ${s.success}`;
        document.getElementById("broken").innerHTML = `Errors: ${s.errors}`;
        document.getElementById("failed").innerHTML = `Failures: ${s.failures}`;
        document.getElementById("inconclusive").innerHTML = `Inconclusive: ${s.inconclusive}`;
        document.getElementById("ignored").innerHTML = `Ignored: ${s.ignored}`;
        document.getElementById("unknown").innerHTML = `Unknown: ${s.unknown}`;
        const pieDiv = document.getElementById("summary-pie");
        Plotly.newPlot(pieDiv, [
            {
                values: [s.success, s.errors, s.failures, s.inconclusive, s.ignored, s.unknown],
                labels: ["Passed", "Broken", "Failed", "Inconclusive", "Ignored", "Unknown"],
                marker: {
                    colors: [
                        Color.passed, Color.broken, Color.failed, Color.inconclusive, Color.ignored, Color.unknown],
                    line: {
                        color: "white",
                        width: 2
                    }
                },
                outsidetextfont: {
                    family: "Helvetica, arial, sans-serif"
                },
                textfont: {
                    family: "Helvetica, arial, sans-serif"
                },
                textinfo: "label+percent",
                type: "pie",
                hole: 0.35
            }
        ], {
            margin: { t: 0 }
        });
    }
    static updateRunPage(runGuid) {
        let run;
        var loader = new JsonLoader();
        loader.loadRunJson(runGuid, PageType.TestRunPage, (response) => {
            run = JSON.parse(response, loader.reviveRun);
            UrlHelper.insertParam("runGuid", run.runInfo.guid);
            RunPageUpdater.updateTime(run);
            RunPageUpdater.updateSummary(run);
            RunPageUpdater.updateName(run);
        });
        return run;
    }
    static loadRun(index = undefined) {
        let runInfos;
        var loader = new JsonLoader();
        loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, loader.reviveRun);
            this.runsCount = runInfos.length;
            if (index === undefined || index.toString() === "NaN") {
                index = this.runsCount - 1;
                this.currentRun = index;
            }
            this.updateRunPage(runInfos[index].guid);
        });
    }
    static tryLoadRunByGuid() {
        const guid = UrlHelper.getParam("runGuid");
        if (guid === "") {
            this.loadRun();
            return;
        }
        let runInfos;
        var loader = new JsonLoader();
        loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, loader.reviveRun);
            this.runsCount = runInfos.length;
            const runInfo = runInfos.find((r) => r.guid === guid);
            if (runInfo != undefined) {
                this.loadRun(runInfos.indexOf(runInfo));
            }
            else {
                this.loadRun();
            }
        });
    }
    static loadPrev() {
        if (this.currentRun === 0) {
            return;
        }
        else {
            this.currentRun -= 1;
            this.loadRun(this.currentRun);
        }
    }
    static loadNext() {
        if (this.currentRun === this.runsCount - 1) {
            return;
        }
        else {
            this.currentRun += 1;
            this.loadRun(this.currentRun);
        }
    }
    static loadLatest() {
        this.loadRun();
    }
    static initialize() {
        this.tryLoadRunByGuid();
    }
    }
    class JsonLoader {
        getRunPath(pt, guid) {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./runs/run_${guid}.json`;
            case PageType.TestRunPage:
                return `./run_${guid}.json`;
            case PageType.TestPage:
                return `./../../runs/run_${guid}.json`;
            default:
                return "";
        }
    }
    loadRunJson(runGuid, pt, callback) {
        const path = this.getRunPath(pt, runGuid);
        this.loadJson(path, callback);
    }
    loadRunsJson(callback) {
        const path = "./runs.json";
        this.loadJson(path, callback);
    }
    loadJson(path, callback) {
        const req = new XMLHttpRequest();
        req.overrideMimeType("application/json");
        req.open("get", path, true);
        req.onreadystatechange = () => {
            if (req.readyState === 4)
                if (req.status !== 200) {
                    console
                        .log(`Error while loading .json data! Request status: ${req.status} : ${req.statusText}`);
                }
                else {
                    callback(req.responseText);
                }
        };
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
    reviveRun(key, value) {
        if (key === "start" || key === "finish")
            return new Date(value);
        return value;
    }
}
function loadRun1(guid) {
    RunPageUpdater.updateRunPage(guid);
}
//# sourceMappingURL=ghpr.controller.js.map