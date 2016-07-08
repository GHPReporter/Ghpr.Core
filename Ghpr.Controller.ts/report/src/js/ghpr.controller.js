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
class PathsHelper {
    static getRunPath(pt, guid) {
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
    static getRunsPath(pt) {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./runs/runs.json`;
            case PageType.TestRunPage:
                return `./runs.json`;
            case PageType.TestPage:
                return `./../../runs/runs.json`;
            default:
                return "";
        }
    }
    static getTestsPath(testGuid, pt) {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./tests/${testGuid}/tests.json`;
            case PageType.TestRunPage:
                return `./../tests/${testGuid}/tests.json`;
            case PageType.TestPage:
                return `./${testGuid}/tests.json`;
            default:
                return "";
        }
    }
    static getTestPath(testGuid, testFileName, pt) {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./tests/${testGuid}/${testFileName}`;
            case PageType.TestRunPage:
                return `./../tests/${testGuid}/${testFileName}`;
            case PageType.TestPage:
                return `./${testGuid}/${testFileName}`;
            default:
                return "";
        }
    }
}
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
            else {
                params.push(p);
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
            else {
                return "";
            }
        }
    }
}
class TabsHelper {
    static showTab(idToShow, caller, pageTabsIds) {
        if (pageTabsIds.indexOf(idToShow) <= -1) {
            return;
        }
        UrlHelper.insertParam("currentTab", idToShow);
        const tabs = document.getElementsByClassName("tabnav-tab");
        for (let i = 0; i < tabs.length; i++) {
            tabs[i].classList.remove("selected");
        }
        caller.className += " selected";
        pageTabsIds.forEach((id) => {
            document.getElementById(id).style.display = "none";
        });
        document.getElementById(idToShow).style.display = "";
    }
}
class JsonLoader {
    constructor(pt) {
        this.pageType = pt;
    }
    loadRunJson(runGuid, callback) {
        const path = PathsHelper.getRunPath(this.pageType, runGuid);
        this.loadJson(path, callback);
    }
    loadRunsJson(callback) {
        const path = PathsHelper.getRunsPath(this.pageType);
        this.loadJson(path, callback);
    }
    loadTestJson(testGuid, testFileName, callback) {
        const path = PathsHelper.getTestPath(testGuid, testFileName, this.pageType);
        this.loadJson(path, callback);
    }
    loadTestsJson(testGuid, callback) {
        const path = PathsHelper.getTestsPath(testGuid, this.pageType);
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
                        .log(`Error while loading .json data: '${path}'! Request status: ${req.status} : ${req.statusText}`);
                }
                else {
                    callback(req.responseText);
                }
        };
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${path}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
    loadJsons(paths, ind, resps, callback) {
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
                if (req.status !== 200) {
                    console
                        .log(`Error while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
                }
                else {
                    resps[ind] = req.responseText;
                    ind++;
                    this.loadJsons(paths, ind, resps, callback);
                }
        };
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
    static reviveRun(key, value) {
        if (key === "start" || key === "finish")
            return new Date(value);
        return value;
    }
}
class DateFormatter {
    static format(date) {
        if (date < new Date(2000, 1)) {
            return "-";
        }
        const year = `${date.getFullYear()}`;
        const month = DateFormatter.correctString(`${date.getMonth() + 1}`);
        const day = DateFormatter.correctString(`${date.getDate()}`);
        const hour = DateFormatter.correctString(`${date.getHours()}`);
        const minute = DateFormatter.correctString(`${date.getMinutes()}`);
        const second = DateFormatter.correctString(`${date.getSeconds()}`);
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
    static correctString(s) {
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
    static updateRunInformation(run) {
        document.getElementById("name").innerHTML = `<b>Run name:</b> ${run.name}`;
        document.getElementById("sprint").innerHTML = `<b>Sprint:</b> ${run.sprint}`;
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(run.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(run.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${DateFormatter.diff(run.runInfo.start, run.runInfo.finish)}`;
    }
    static updateTitle(run) {
        document.getElementById("page-title").innerHTML = run.name;
    }
    static updateSummary(run) {
        const s = run.summary;
        document.getElementById("total").innerHTML = `<b>Total:</b> ${s.total}`;
        document.getElementById("passed").innerHTML = `<b>Success:</b> ${s.success}`;
        document.getElementById("broken").innerHTML = `<b>Errors:</b> ${s.errors}`;
        document.getElementById("failed").innerHTML = `<b>Failures:</b> ${s.failures}`;
        document.getElementById("inconclusive").innerHTML = `<b>Inconclusive:</b> ${s.inconclusive}`;
        document.getElementById("ignored").innerHTML = `<b>Ignored:</b> ${s.ignored}`;
        document.getElementById("unknown").innerHTML = `<b>Unknown:</b> ${s.unknown}`;
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
    static setTestsList(tests) {
        let list = "";
        const c = tests.length;
        for (let i = 0; i < c; i++) {
            const t = tests[i];
            list += `<li id=$test-${t.testInfo.guid}>Test #${c - i - 1}: <a href="./../tests/index.html?testGuid=${t.testInfo.guid}&testFile=${t.testInfo.fileName}">${t.name}</a></li>`;
        }
        document.getElementById("all-tests").innerHTML = list;
    }
    static updateRunPage(runGuid) {
        let run;
        this.loader.loadRunJson(runGuid, (response) => {
            run = JSON.parse(response, JsonLoader.reviveRun);
            UrlHelper.insertParam("runGuid", run.runInfo.guid);
            RunPageUpdater.updateRunInformation(run);
            this.updateSummary(run);
            RunPageUpdater.updateTitle(run);
            this.updateTestsList(run);
        });
        return run;
    }
    static updateTestsList(run) {
        const paths = new Array();
        const testStrings = new Array();
        const tests = new Array();
        document.getElementById("btn-back").setAttribute("href", `./../index.html`);
        const files = run.testRunFiles;
        for (let i = 0; i < files.length; i++) {
            paths[i] = `./../tests/${files[i]}`;
        }
        this.loader.loadJsons(paths, 0, testStrings, (responses) => {
            for (let i = 0; i < responses.length; i++) {
                tests[i] = JSON.parse(responses[i], JsonLoader.reviveRun);
            }
            this.setTestsList(tests);
        });
    }
    static loadRun(index) {
        let runInfos;
        this.loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, JsonLoader.reviveRun);
            runInfos.sort(Sorter.itemInfoSorterByFinishDateFunc);
            this.runsCount = runInfos.length;
            if (index === undefined || index.toString() === "NaN") {
                index = this.runsCount - 1;
            }
            if (index === 0) {
                this.disableBtn("btn-prev");
            }
            if (index === runInfos.length - 1) {
                this.disableBtn("btn-next");
            }
            this.currentRun = index;
            this.updateRunPage(runInfos[index].guid);
        });
    }
    static tryLoadRunByGuid() {
        const guid = UrlHelper.getParam("runGuid");
        if (guid === "") {
            this.loadRun(undefined);
            return;
        }
        let runInfos;
        this.loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, JsonLoader.reviveRun);
            runInfos.sort(Sorter.itemInfoSorterByFinishDateFunc);
            this.runsCount = runInfos.length;
            const runInfo = runInfos.find((r) => r.guid === guid);
            if (runInfo != undefined) {
                this.enableBtns();
                const index = runInfos.indexOf(runInfo);
                if (index === 0) {
                    this.disableBtn("btn-prev");
                }
                if (index === runInfos.length - 1) {
                    this.disableBtn("btn-next");
                }
                this.loadRun(index);
            }
            else {
                this.loadRun(undefined);
            }
        });
    }
    static enableBtns() {
        document.getElementById("btn-prev").removeAttribute("disabled");
        document.getElementById("btn-next").removeAttribute("disabled");
    }
    static disableBtn(id) {
        document.getElementById(id).setAttribute("disabled", "true");
    }
    static loadPrev() {
        if (this.currentRun === 0) {
            this.disableBtn("btn-prev");
            return;
        }
        else {
            this.enableBtns();
            this.currentRun -= 1;
            if (this.currentRun === 0) {
                this.disableBtn("btn-prev");
            }
            this.loadRun(this.currentRun);
        }
    }
    static loadNext() {
        if (this.currentRun === this.runsCount - 1) {
            this.disableBtn("btn-next");
            return;
        }
        else {
            this.enableBtns();
            this.currentRun += 1;
            if (this.currentRun === this.runsCount - 1) {
                this.disableBtn("btn-next");
            }
            this.loadRun(this.currentRun);
        }
    }
    static loadLatest() {
        this.disableBtn("btn-next");
        this.loadRun(undefined);
    }
    static initializePage() {
        this.tryLoadRunByGuid();
        const tabFromUrl = UrlHelper.getParam("currentTab");
        const tab = tabFromUrl === "" ? "run-main-stats" : tabFromUrl;
        this.showTab(tab, document.getElementById(`tab-${tab}`));
    }
    static showTab(idToShow, caller) {
        TabsHelper.showTab(idToShow, caller, this.runPageTabsIds);
    }
}
RunPageUpdater.loader = new JsonLoader(PageType.TestRunPage);
RunPageUpdater.runPageTabsIds = ["run-main-stats", "run-test-list"];
class ReportPageUpdater {
    static updateFields(run) {
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(run.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(run.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${DateFormatter.diff(run.runInfo.start, run.runInfo.finish)}`;
    }
    static updateRunsList(runs) {
        let list = "";
        const c = runs.length;
        for (let i = 0; i < c; i++) {
            const r = runs[i];
            list += `<li id=$run-${r.runInfo.guid}>Run #${c - i - 1}: <a href="./runs/index.html?runGuid=${r.runInfo.guid}">${r.name}</a></li>`;
        }
        document.getElementById("all-runs").innerHTML = list;
    }
    static updatePlotlyBars(runs) {
        document.getElementById("total").innerHTML = `<b>Total:</b> ${runs.length}`;
        let plotlyData = new Array();
        const passedY = new Array();
        const failedY = new Array();
        const brokenY = new Array();
        const inconclY = new Array();
        const ignoredY = new Array();
        const unknownY = new Array();
        const passedX = new Array();
        const failedX = new Array();
        const brokenX = new Array();
        const inconclX = new Array();
        const ignoredX = new Array();
        const unknownX = new Array();
        const tickvals = new Array();
        const ticktext = new Array();
        const c = runs.length;
        for (let i = 0; i < c; i++) {
            let s = runs[i].summary;
            passedY[i] = s.success;
            failedY[i] = s.failures;
            brokenY[i] = s.errors;
            inconclY[i] = s.inconclusive;
            ignoredY[i] = s.ignored;
            unknownY[i] = s.unknown;
            let j = c - i - 1;
            passedX[i] = j;
            failedX[i] = j;
            brokenX[i] = j;
            inconclX[i] = j;
            ignoredX[i] = j;
            unknownX[i] = j;
            tickvals[i] = j;
            ticktext[i] = `run ${j}`;
        }
        const t = "bar";
        const hi = "y";
        plotlyData = [
            { x: unknownX, y: unknownY, name: "unknown", type: t, hoverinfo: hi, marker: { color: Color.unknown } },
            { x: inconclX, y: inconclY, name: "inconclusive", type: t, hoverinfo: hi, marker: { color: Color.inconclusive } },
            { x: ignoredX, y: ignoredY, name: "ignored", type: t, hoverinfo: hi, marker: { color: Color.ignored } },
            { x: brokenX, y: brokenY, name: "broken", type: t, hoverinfo: hi, marker: { color: Color.broken } },
            { x: failedX, y: failedY, name: "failed", type: t, hoverinfo: hi, marker: { color: Color.failed } },
            { x: passedX, y: passedY, name: "passed", type: t, hoverinfo: hi, marker: { color: Color.passed } }
        ];
        const pieDiv = document.getElementById("runs-bars");
        Plotly.newPlot(pieDiv, plotlyData, {
            title: "Runs statistics",
            xaxis: {
                tickvals: tickvals,
                ticktext: ticktext,
                title: "Runs"
            },
            yaxis: {
                title: "Tests number"
            },
            barmode: "stack",
            bargap: 0.01
        });
    }
    static updatePage(index) {
        let runInfos;
        const paths = new Array();
        const r = new Array();
        const runs = new Array();
        this.loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, JsonLoader.reviveRun);
            for (let i = 0; i < runInfos.length; i++) {
                paths[i] = `runs/run_${runInfos[i].guid}.json`;
            }
            this.loader.loadJsons(paths, 0, r, (responses) => {
                for (let i = 0; i < responses.length; i++) {
                    runs[i] = JSON.parse(responses[i], JsonLoader.reviveRun);
                }
                this.updateFields(runs[runs.length - 1]);
                this.updatePlotlyBars(runs);
                this.updateRunsList(runs);
            });
        });
    }
    static initializePage() {
        this.updatePage(undefined);
        this.showTab("runs-stats", document.getElementById("tab-runs-stats"));
    }
    static showTab(idToShow, caller) {
        TabsHelper.showTab(idToShow, caller, this.reportPageTabsIds);
    }
}
ReportPageUpdater.loader = new JsonLoader(PageType.TestRunsPage);
ReportPageUpdater.reportPageTabsIds = ["runs-stats", "runs-list"];
class TestRunHelper {
    static getColor(t) {
        const result = this.getResult(t);
        switch (result) {
            case TestResult.Passed:
                return Color.passed;
            case TestResult.Failed:
                return Color.failed;
            case TestResult.Broken:
                return Color.broken;
            case TestResult.Ignored:
                return Color.ignored;
            case TestResult.Inconclusive:
                return Color.inconclusive;
            default:
                return Color.unknown;
        }
    }
    static getResult(t) {
        if (t.result.indexOf("Passed") > -1) {
            return TestResult.Passed;
        }
        if (t.result.indexOf("Error") > -1) {
            return TestResult.Broken;
        }
        if (t.result.indexOf("Failed") > -1 || t.result.indexOf("Failure") > -1) {
            return TestResult.Failed;
        }
        if (t.result.indexOf("Inconclusive") > -1) {
            return TestResult.Inconclusive;
        }
        if (t.result.indexOf("Ignored") > -1 || t.result.indexOf("Skipped") > -1) {
            return TestResult.Ignored;
        }
        return TestResult.Unknown;
    }
    static getColoredResult(t) {
        return `<span class="p-1" style= "background-color: ${this.getColor(t)};" > ${t.result} </span>`;
    }
    static getOutput(t) {
        return t.output === "" ? "-" : t.output;
    }
    static getMessage(t) {
        return t.testMessage === "" ? "-" : t.testMessage;
    }
    static getStackTrace(t) {
        return t.testStackTrace === "" ? "-" : t.testStackTrace;
    }
    static getCategories(t) {
        return t.categories.length <= 0 ? "-" : t.categories.join(", ");
    }
}
class TestPageUpdater {
    static updateMainInformation(t) {
        document.getElementById("page-title").innerHTML = `<b>Test:</b> ${t.name}`;
        document.getElementById("name").innerHTML = `<b>Test name:</b> ${t.name}`;
        document.getElementById("full-name").innerHTML = `<b>Full name:</b> ${t.fullName}`;
        document.getElementById("result").innerHTML = `<b>Result:</b> ${TestRunHelper.getColoredResult(t)}`;
        document.getElementById("priority").innerHTML = `<b>Priority:</b> ${t.priority}`;
        document.getElementById("test-type").innerHTML = `<b>Test type:</b> ${t.testType}`;
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(t.testInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(t.testInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${t.duration.toString()}`;
        document.getElementById("categories").innerHTML = `<b>Categories:</b> ${TestRunHelper.getCategories(t)}`;
        document.getElementById("message").innerHTML = `<b>Message:</b> ${TestRunHelper.getMessage(t)}`;
    }
    static updateOutput(t) {
        document.getElementById("test-output-string").innerHTML = `<b>Test log:</b><br> ${TestRunHelper.getOutput(t)}`;
    }
    static updateFailure(t) {
        document.getElementById("test-message").innerHTML = `<b>Message:</b><br> ${TestRunHelper.getMessage(t)}`;
        document.getElementById("test-stack-trace").innerHTML = `<b>Stack trace:</b><br> ${TestRunHelper.getStackTrace(t)}`;
    }
    static setTestHistory(tests) {
        const historyDiv = document.getElementById("test-history-chart");
        let plotlyData = new Array();
        const dataX = new Array();
        const dataY = new Array();
        const tickvals = new Array();
        const ticktext = new Array();
        const colors = new Array();
        const c = tests.length;
        for (let i = 0; i < c; i++) {
            const t = tests[i];
            dataX[i] = t.testInfo.finish;
            colors[i] = TestRunHelper.getColor(t);
            const j = c - i - 1;
            dataY[i] = t.duration;
            tickvals[i] = j;
            ticktext[i] = `test ${j}`;
        }
        const historyTrace = {
            x: dataX,
            y: dataY,
            name: "Test history",
            hoverinfo: "x",
            type: "scatter",
            showlegend: false,
            marker: {
                color: colors,
                size: 25,
                line: { color: Color.unknown, width: 4 }
            },
            mode: "lines+markers",
            line: { shape: "spline", color: Color.unknown, width: 8 },
            textfont: { family: "Helvetica, arial, sans-serif" }
        };
        const index = this.currentTest;
        const currentTest = {
            x: [dataX[index]],
            y: [dataY[index]],
            name: "Current test",
            type: "scatter",
            mode: "markers",
            hoverinfo: "name",
            showlegend: false,
            marker: {
                color: [TestRunHelper.getColor(tests[index])],
                size: 40,
                line: { color: Color.unknown, width: 8 }
            }
        };
        plotlyData = [historyTrace, currentTest];
        const layout = {
            title: "Test history",
            xaxis: {
                title: "Finish datetime"
            },
            yaxis: {
                title: "Test duration (sec.)"
            }
        };
        Plotly.newPlot(historyDiv, plotlyData, layout);
    }
    static updateTestPage(testGuid, fileName) {
        let test;
        this.loader.loadTestJson(testGuid, fileName, (response) => {
            test = JSON.parse(response, JsonLoader.reviveRun);
            UrlHelper.insertParam("testGuid", test.testInfo.guid);
            UrlHelper.insertParam("testFile", test.testInfo.fileName);
            this.updateMainInformation(test);
            this.updateOutput(test);
            this.updateFailure(test);
            document.getElementById("btn-back").setAttribute("href", `./../runs/index.html?runGuid=${test.runGuid}`);
            this.updateTestHistory();
        });
        return test;
    }
    static updateTestHistory() {
        const paths = new Array();
        const testStrings = new Array();
        const tests = new Array();
        const guid = UrlHelper.getParam("testGuid");
        let testInfos;
        this.loader.loadTestsJson(guid, (response) => {
            testInfos = JSON.parse(response, JsonLoader.reviveRun);
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFunc);
            for (let i = 0; i < testInfos.length; i++) {
                paths[i] = `./${testInfos[i].guid}/${testInfos[i].fileName}`;
            }
            this.loader.loadJsons(paths, 0, testStrings, (responses) => {
                for (let i = 0; i < responses.length; i++) {
                    tests[i] = JSON.parse(responses[i], JsonLoader.reviveRun);
                }
                this.setTestHistory(tests);
            });
        });
    }
    static loadTest(index) {
        const guid = UrlHelper.getParam("testGuid");
        let testInfos;
        this.loader.loadTestsJson(guid, (response) => {
            testInfos = JSON.parse(response, JsonLoader.reviveRun);
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFunc);
            this.testVersionsCount = testInfos.length;
            if (index === undefined || index.toString() === "NaN") {
                index = this.testVersionsCount - 1;
            }
            if (index === 0) {
                this.disableBtn("btn-prev");
            }
            if (index === testInfos.length - 1) {
                this.disableBtn("btn-next");
            }
            this.currentTest = index;
            this.updateTestPage(testInfos[index].guid, testInfos[index].fileName);
        });
    }
    static tryLoadTestByGuid() {
        const guid = UrlHelper.getParam("testGuid");
        const fileName = UrlHelper.getParam("testFile");
        let testInfos;
        this.loader.loadTestsJson(guid, (response) => {
            testInfos = JSON.parse(response, JsonLoader.reviveRun);
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFunc);
            this.testVersionsCount = testInfos.length;
            const testInfo = testInfos.find((t) => t.fileName === fileName);
            if (testInfo != undefined) {
                this.enableBtns();
                const index = testInfos.indexOf(testInfo);
                if (index === 0) {
                    this.disableBtn("btn-prev");
                }
                if (index === testInfos.length - 1) {
                    this.disableBtn("btn-next");
                }
                this.loadTest(index);
            }
            else {
                this.loadTest(undefined);
            }
        });
    }
    static enableBtns() {
        document.getElementById("btn-prev").removeAttribute("disabled");
        document.getElementById("btn-next").removeAttribute("disabled");
    }
    static disableBtn(id) {
        document.getElementById(id).setAttribute("disabled", "true");
    }
    static loadPrev() {
        if (this.currentTest === 0) {
            this.disableBtn("btn-prev");
            return;
        }
        else {
            this.enableBtns();
            this.currentTest -= 1;
            if (this.currentTest === 0) {
                this.disableBtn("btn-prev");
            }
            this.loadTest(this.currentTest);
        }
    }
    static loadNext() {
        if (this.currentTest === this.testVersionsCount - 1) {
            this.disableBtn("btn-next");
            return;
        }
        else {
            this.enableBtns();
            this.currentTest += 1;
            if (this.currentTest === this.testVersionsCount - 1) {
                this.disableBtn("btn-next");
            }
            this.loadTest(this.currentTest);
        }
    }
    static loadLatest() {
        this.disableBtn("btn-next");
        this.loadTest(undefined);
    }
    static initializePage() {
        this.tryLoadTestByGuid();
        const tabFromUrl = UrlHelper.getParam("currentTab");
        const tab = tabFromUrl === "" ? "test-history" : tabFromUrl;
        this.showTab(tab === "" ? "test-history" : tab, document.getElementById(`tab-${tab}`));
    }
    static showTab(idToShow, caller) {
        TabsHelper.showTab(idToShow, caller, this.runPageTabsIds);
    }
}
TestPageUpdater.loader = new JsonLoader(PageType.TestPage);
TestPageUpdater.runPageTabsIds = ["test-history", "test-output", "test-failure", "test-screenshots"];
class Sorter {
    static itemInfoSorterByFinishDateFunc(a, b) {
        if (a.finish > b.finish) {
            return 1;
        }
        if (a.finish < b.finish) {
            return -1;
        }
        return 0;
    }
}
function loadRun1(guid) {
}
//# sourceMappingURL=ghpr.controller.js.map