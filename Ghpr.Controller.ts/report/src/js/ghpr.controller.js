class Color {
}
Color.passed = "#8bc34a";
Color.broken = "#ffc107";
Color.failed = "#ef5350";
Color.ignored = "#81d4fa";
Color.inconclusive = "#D6FAF7";
Color.unknown = "#bdbdbd";
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
class TestRunHelper {
    static getColorByResult(result) {
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
            case TestResult.Unknown:
                return Color.unknown;
            default:
                return "white";
        }
    }
    static getColor(t) {
        const result = this.getResult(t);
        return this.getColorByResult(result);
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
    static getColoredIns(v) {
        return `<ins class="p-0" style= "background-color: ${Color.passed};text-decoration: none;" >${v}</ins>`;
    }
    static getColoredDel(v) {
        return `<del class="p-0" style= "background-color: ${Color.failed};text-decoration: none;" >${v}</del>`;
    }
    static getOutput(t) {
        return t.output === "" ? "-" : t.output;
    }
    static getMessage(t) {
        return t.testMessage === "" ? "-" : t.testMessage;
    }
    static getDescription(t) {
        return (t.description === "" || t.description === undefined) ? "-" : t.description;
    }
    static getStackTrace(t) {
        return t.testStackTrace === "" ? "-" : t.testStackTrace;
    }
    static getCategories(t) {
        if (t.categories === undefined) {
            return "-";
        }
        return t.categories.length <= 0 ? "-" : t.categories.join(", ");
    }
}
class Differ {
    static splitInclusive(str, sep, trim) {
        if (!str.length) {
            return [];
        }
        let split = str.split(sep);
        if (trim) {
            split = split.filter((v) => v.length);
        }
        return split.map((line, idx, arr) => (idx < arr.length - 1 ? line + sep : line));
    }
    static splitInclusiveFull(str, sep, trim) {
        if (!str.length) {
            return [];
        }
        let split = str.split(sep);
        if (trim) {
            split = split.filter((v) => v.length);
        }
        let res = new Array();
        split.forEach((s, i, a) => {
            if (i < a.length - 1) {
                res.push(s);
                res.push(sep);
            }
            else {
                res.push(s);
            }
        });
        return res;
    }
    static splitArrInclusive(strs, sep, trim) {
        if (!strs.length) {
            return [];
        }
        let res = new Array();
        strs.forEach((str, index, arr) => {
            const splittedStr = this.splitInclusiveFull(str, sep, trim);
            splittedStr.forEach((s, i, a) => {
                res.push(s);
            });
        });
        return res;
    }
    static splitInclusiveSeveral(str, seps, trim) {
        if (!str.length) {
            return [];
        }
        let res = str;
        seps.forEach((sep, index, arr) => {
            const splittedStr = this.splitArrInclusive(res, sep, trim);
            res = splittedStr;
        });
        return res;
    }
    static flattenRepeats(acc, cur) {
        if (acc.length && acc[acc.length - 1].value === cur) {
            acc[acc.length - 1].count++;
            return acc;
        }
        return acc.concat({
            value: cur,
            ref: -1,
            count: 1
        });
    }
    static addToTable(table, arr, type) {
        arr.forEach((token, idx) => {
            var ref = table;
            ref = ref[token.value] || (ref[token.value] = {});
            ref = ref[token.count] || (ref[token.count] = {
                left: -1,
                right: -1
            });
            if (ref[type] === -1) {
                ref[type] = idx;
            }
            else if (ref[type] >= 0) {
                ref[type] = -2;
            }
        });
    }
    static findUnique(table, left, right) {
        left.forEach((token) => {
            var ref = table[token.value][token.count];
            if (ref.left >= 0 && ref.right >= 0) {
                left[ref.left].ref = ref.right;
                right[ref.right].ref = ref.left;
            }
        });
    }
    static expandUnique(table, left, right, dir) {
        left.forEach((token, idx) => {
            if (token.ref === -1) {
                return;
            }
            var i = idx + dir, j = token.ref + dir, lx = left.length, rx = right.length;
            while (i >= 0 && j >= 0 && i < lx && j < rx) {
                if (left[i].value !== right[j].value) {
                    break;
                }
                left[i].ref = j;
                right[j].ref = i;
                i += dir;
                j += dir;
            }
        });
    }
    static push(acc, token, type) {
        let n = token.count;
        while (n--) {
            acc.push({ type: type, value: token.value });
        }
    }
    static calcDist(lTarget, lPos, rTarget, rPos) {
        return (lTarget - lPos) + (rTarget - rPos) + Math.abs((lTarget - lPos) - (rTarget - rPos));
    }
    static processDiff(left, right) {
        var acc = [], lPos = 0, rPos = 0, lx = left.length, rx = right.length, lToken, rToken, lTarget, rTarget, rSeek, dist1, dist2;
        var countDiff;
        while (lPos < lx) {
            lTarget = lPos;
            while (left[lTarget].ref < 0) {
                lTarget++;
            }
            rTarget = left[lTarget].ref;
            if (rTarget < rPos) {
                while (lPos < lTarget) {
                    this.push(acc, left[lPos++], "del");
                }
                this.push(acc, left[lPos++], "del");
                continue;
            }
            rToken = right[rTarget];
            dist1 = this.calcDist(lTarget, lPos, rTarget, rPos);
            for (rSeek = rTarget - 1; dist1 > 0 && rSeek >= rPos; rSeek--) {
                if (right[rSeek].ref < 0) {
                    continue;
                }
                if (right[rSeek].ref < lPos) {
                    continue;
                }
                dist2 = this.calcDist(right[rSeek].ref, lPos, rSeek, rPos);
                if (dist2 < dist1) {
                    dist1 = dist2;
                    rTarget = rSeek;
                    lTarget = right[rSeek].ref;
                }
            }
            while (lPos < lTarget) {
                this.push(acc, left[lPos++], "del");
            }
            while (rPos < rTarget) {
                this.push(acc, right[rPos++], "ins");
            }
            if ("eof" in left[lPos]) {
                break;
            }
            countDiff = left[lPos].count - right[rPos].count;
            if (countDiff === 0) {
                this.push(acc, left[lPos], "same");
            }
            else if (countDiff < 0) {
                this.push(acc, {
                    count: right[rPos].count + countDiff,
                    value: right[rPos].value
                }, "same");
                this.push(acc, {
                    count: -countDiff,
                    value: right[rPos].value
                }, "ins");
            }
            else if (countDiff > 0) {
                this.push(acc, {
                    count: left[lPos].count - countDiff,
                    value: left[lPos].value
                }, "same");
                this.push(acc, {
                    count: countDiff,
                    value: left[lPos].value
                }, "del");
            }
            lPos++;
            rPos++;
        }
        return acc;
    }
    static same(left, right) {
        if (left.length !== right.length) {
            return false;
        }
        return left.reduce((acc, cur, idx) => (acc && cur === right[idx]), true);
    }
    ;
    static all(type) {
        return (val) => ({
            type: type,
            value: val
        });
    }
    static diff(leftLines, rightLines) {
        let left = (leftLines && Array.isArray(leftLines) ? leftLines : []);
        let right = (rightLines && Array.isArray(rightLines) ? rightLines : []);
        if (this.same(leftLines, rightLines)) {
            return left.map(this.all("same"));
        }
        if (left.length === 0) {
            return right.map(this.all("ins"));
        }
        if (right.length === 0) {
            return left.map(this.all("del"));
        }
        left = left.reduce(this.flattenRepeats, []);
        right = right.reduce(this.flattenRepeats, []);
        let table = {};
        this.addToTable(table, left, "left");
        this.addToTable(table, right, "right");
        this.findUnique(table, left, right);
        this.expandUnique(table, left, right, 1);
        this.expandUnique(table, left, right, -1);
        left.push({ ref: right.length, eof: true });
        table = null;
        const res = this.processDiff(left, right);
        left = null;
        right = null;
        return res;
    }
    static accumulateChanges(changes, fn) {
        var del = [], ins = [];
        changes.forEach((change) => {
            if (change.type === "del") {
                del.push(change.value);
            }
            if (change.type === "ins") {
                ins.push(change.value);
            }
        });
        if (!del.length || !ins.length) {
            return changes;
        }
        return fn(del.join(""), ins.join(""));
    }
    static refineChanged(changes, fn) {
        var ptr = -1;
        return changes.concat({
            type: "same",
            eof: true
        }).reduce((acc, cur, idx, a) => {
            var part = [];
            if (cur.type === "same") {
                if (ptr >= 0) {
                    part = this.accumulateChanges(a.slice(ptr, idx), fn);
                    if (a[idx - 1].type !== "ins") {
                        part = a.slice(ptr, idx);
                    }
                    else {
                    }
                    ptr = -1;
                }
                return acc.concat(part).concat(cur.eof ? [] : [cur]);
            }
            else if (ptr < 0) {
                ptr = idx;
            }
            return acc;
        }, []);
    }
    static minimize(changes) {
        var del = [], ins = [];
        return changes.concat({ type: "same", eof: true })
            .reduce((acc, cur) => {
            if (cur.type === "del") {
                del.push(cur.value);
                return acc;
            }
            if (cur.type === "ins") {
                ins.push(cur.value);
                return acc;
            }
            if (del.length) {
                acc.push({
                    type: "del",
                    value: del.join("")
                });
                del = [];
            }
            if (ins.length) {
                acc.push({
                    type: "ins",
                    value: ins.join("")
                });
                ins = [];
            }
            if (cur.eof !== true) {
                if (acc.length && acc[acc.length - 1].type === "same") {
                    acc[acc.length - 1].value += cur.value;
                }
                else {
                    acc.push(cur);
                }
            }
            return acc;
        }, []);
    }
    static diffLines(left, right, trim) {
        return this.diff(this.splitInclusiveFull(left, "\n", trim), this.splitInclusiveFull(right, "\n", trim));
    }
    static diffWords(left, right, trim) {
        return this.diff(this.splitInclusiveFull(left, " ", trim), this.splitInclusiveFull(right, " ", trim));
    }
    static diffLetters(left, right, trim) {
        return this.diff(this.splitInclusiveSeveral([left], this.separators, trim), this.splitInclusiveSeveral([right], this.separators, trim));
    }
    static diffChars(left, right, trim) {
        return this.diff(this.splitInclusiveFull(left, "", trim), this.splitInclusiveFull(right, "", trim));
    }
    static diffHybrid(left, right, trim) {
        return this.refineChanged(this.diffLines(left, right, trim), (del, ins) => this.diffLetters(del, ins, trim));
    }
    static replaceTag(tag) {
        const tagsToReplace = {
            '&': "&amp;",
            '<': "&lt;",
            '>': "&gt;",
            '"': "&quot;",
            "'": "&#39;",
            '/': "&#x2F;",
            '`': "&#x60;",
            '=': "&#x3D;"
        };
        return tagsToReplace[tag] || tag;
    }
    static safeTagsReplace(str) {
        return str.replace(/[&<>]/g, this.replaceTag);
    }
    static getHtmlForOneChange(change) {
        let res = "";
        const v = this.safeTagsReplace(change.value);
        if (change.value === "") {
            res = "";
        }
        if (change.type === "same") {
            res = `${v}`;
        }
        if (change.type === "ins") {
            res = TestRunHelper.getColoredIns(v);
        }
        if (change.type === "del") {
            res = TestRunHelper.getColoredDel(v);
        }
        return res;
    }
    static getHtml(left, right) {
        let res = "";
        const changes = this.diffHybrid(left, right, false);
        changes.forEach((change) => {
            res += this.getHtmlForOneChange(change);
        });
        res = `<div style="word-wrap: break-word;  white-space: pre-wrap;">${res}</div>`;
        return res;
    }
}
Differ.separators = [" ", "<", ">", "/", ".", "?", "!"];
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
    static removeParam(key) {
        const paramsPart = document.location.search.substr(1);
        window.history.pushState("", "", "");
        if (paramsPart === "") {
            return;
        }
        else {
            let params = paramsPart.split("&");
            const paramToRemove = params.find((par) => par.split("=")[0] === key);
            if (paramToRemove != undefined) {
                const index = params.indexOf(paramToRemove);
                params.splice(index, 1);
            }
            window.history.pushState("", "", `?${params.join("&")}`);
        }
    }
}
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
    static getReportSettingsPath(pt) {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./src/ReportSettings.json`;
            case PageType.TestRunPage:
                return `./../src/ReportSettings.json`;
            case PageType.TestPage:
                return `./../src/ReportSettings.json`;
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
class ProgressBar {
    constructor(total) {
        this.barId = "progress-bar";
        this.barDivId = "progress-bar-div";
        this.barTextId = "progress-bar-line";
        this.reset(total);
    }
    reset(total) {
        this.total = total;
        this.current = 0;
    }
    show() {
        document.getElementById(this.barId).style.display = "";
        document.getElementById(this.barId).innerHTML = `<div id="${this.barDivId}"><div id="${this.barTextId}"></div></div>`;
        document.getElementById(this.barId).style.position = "relative";
        document.getElementById(this.barId).style.width = "100%";
        document.getElementById(this.barId).style.height = "20px";
        document.getElementById(this.barId).style.backgroundColor = Color.unknown;
        document.getElementById(this.barDivId).style.position = "absolute";
        document.getElementById(this.barDivId).style.width = "10%";
        document.getElementById(this.barDivId).style.height = "100%";
        document.getElementById(this.barDivId).style.backgroundColor = Color.passed;
        document.getElementById(this.barTextId).style.textAlign = "center";
        document.getElementById(this.barTextId).style.lineHeight = "20px";
        document.getElementById(this.barTextId).style.color = "white";
    }
    onLoaded(count) {
        this.current += count;
        const percentage = 100 * this.current / this.total;
        const pString = percentage.toString().split(".")[0] + "%";
        document.getElementById(this.barDivId).style.width = pString;
        document.getElementById(this.barTextId).innerHTML = pString;
    }
    hide() {
        document.getElementById(this.barId).innerHTML = "";
        document.getElementById(this.barId).style.display = "none";
    }
}
class JsonLoader {
    constructor(pt) {
        this.pageType = pt;
        this.progressBar = new ProgressBar(1);
    }
    loadRunJson(runGuid, callback) {
        const path = PathsHelper.getRunPath(this.pageType, runGuid);
        this.loadJson(path, callback);
    }
    loadRunsJson(callback) {
        const path = PathsHelper.getRunsPath(this.pageType);
        this.loadJson(path, callback);
    }
    loadReportSettingsJson(callback) {
        const path = PathsHelper.getReportSettingsPath(this.pageType);
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
                if (req.status !== 200 && req.status !== 0) {
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
    loadAllJsons(paths, ind, resps, callback) {
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
                }
                else {
                    resps[ind] = req.responseText;
                    ind++;
                    this.loadAllJsons(paths, ind, resps, callback);
                }
        };
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
    loadJsons(paths, ind, callback) {
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
                }
                else {
                    callback(req.responseText, count, ind);
                    this.progressBar.onLoaded(ind);
                    ind++;
                    this.loadJsons(paths, ind, callback);
                }
        };
        req.timeout = 2000;
        req.ontimeout = () => {
            console.log(`Timeout while loading .json data: '${paths[ind]}'! Request status: ${req.status} : ${req.statusText}`);
        };
        req.send(null);
    }
    static reviveRun(key, value) {
        if (key === "start" || key === "finish" || key === "date")
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
        const month = this.correctString(`${date.getMonth() + 1}`);
        const day = this.correctString(`${date.getDate()}`);
        const hour = this.correctString(`${date.getHours()}`);
        const minute = this.correctString(`${date.getMinutes()}`);
        const second = this.correctString(`${date.getSeconds()}`);
        return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
    }
    static diff(start, finish) {
        const timeDifference = (finish.getTime() - start.getTime());
        const dDate = new Date(timeDifference);
        const dHours = dDate.getUTCHours();
        const dMins = dDate.getUTCMinutes();
        const dSecs = dDate.getUTCSeconds();
        const dMilliSecs = dDate.getUTCMilliseconds();
        const readableDifference = this.correctNumber(dHours) + ":" + this.correctNumber(dMins) + ":"
            + this.correctNumber(dSecs) + "." + this.correctNumber(dMilliSecs);
        return readableDifference;
    }
    static correctString(s) {
        if (s.length === 1) {
            return `0${s}`;
        }
        else
            return s;
    }
    static correctNumber(n) {
        if (n >= 0 && n < 10) {
            return `0${n}`;
        }
        else
            return `${n}`;
    }
}
class RunPageUpdater {
    static updateCopyright() {
        document.getElementById("copyright").innerHTML = `Copyright 2015 - 2017 © GhpReporter (version ${this.reportSettings.coreVersion})`;
    }
    static updateRunInformation(run) {
        document.getElementById("name").innerHTML = `<b>Name:</b> ${run.name}`;
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
                        Color.passed, Color.broken, Color.failed, Color.inconclusive, Color.ignored, Color.unknown
                    ],
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
    static addTest(t, c, i) {
        const ti = t.testInfo;
        const testHref = `./../tests/index.html?testGuid=${ti.guid}&testFile=${ti.fileName}`;
        const testLi = `<li id="test-${ti.guid}" style="list-style-type: none;" class="${TestRunHelper.getResult(t)}">
            <span class="octicon octicon-primitive-square" style="color: ${TestRunHelper.getColor(t)};"></span>
            <a href="${testHref}"> ${t.name}</a></li>`;
        const nameIndex = t.fullName.lastIndexOf(t.name);
        let nameRemoved = false;
        let fn = t.fullName;
        if (t.fullName.indexOf(t.name) > 0) {
            fn = t.fullName.substring(0, nameIndex);
            nameRemoved = true;
        }
        if (fn.slice(-1) === ".") {
            fn = fn.slice(0, -1);
        }
        const arr = fn.split(".");
        const len1 = arr.length;
        for (let j = arr.length - 1; j >= 0; j -= 1) {
            if (/[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(arr[j])) {
                arr.splice(j, 1);
            }
        }
        let len2 = arr.length;
        if ((len1 === len2) && !nameRemoved) {
            arr.splice(len2 - 1, 1);
            len2 = arr.length;
        }
        const ids = new Array();
        for (let j = 0; j < len2; j++) {
            ids[j] = `id-${arr.slice(0, j + 1).join(".").replace(/\s/g, "_")}`;
        }
        for (let j = 0; j <= len2; j++) {
            const el = document.getElementById(ids[j]);
            if (el === null || el === undefined) {
                const li = `<li id="${ids[j]}" class="test-suite"><a>${arr[j]}</a><ul></ul></li>`;
                if (j === 0) {
                    document.getElementById("all-tests").innerHTML += li;
                }
                else {
                    if (j !== len2) {
                        document.getElementById(ids[j - 1]).getElementsByTagName("ul")[0].innerHTML += li;
                    }
                    else {
                        document.getElementById(ids[j - 1]).getElementsByTagName("ul")[0].innerHTML += testLi;
                    }
                }
            }
        }
    }
    static makeCollapsible() {
        const targets = document.getElementsByClassName("test-suite");
        for (let i = 0; i < targets.length; i++) {
            const t = targets[i];
            t.getElementsByTagName("a")[0].onclick = () => {
                const e = t.getElementsByTagName("ul")[0];
                if (e.style.display === "") {
                    e.style.display = "none";
                }
                else {
                    e.style.display = "";
                }
            };
        }
    }
    static updateTestFilterButtons() {
        const btns = document.getElementById("test-result-filter-buttons").getElementsByTagName("button");
        for (let i = 0; i < btns.length; i++) {
            const btn = btns[i];
            const id = btn.getAttribute("id");
            const tests = document.getElementsByClassName(id);
            btn.style.backgroundImage = "none";
            btn.style.backgroundColor = TestRunHelper.getColorByResult(Number(id));
            btn.onclick = () => {
                if (!btn.classList.contains("disabled")) {
                    btn.classList.add("disabled");
                    for (let j = 0; j < tests.length; j++) {
                        const t = tests[j];
                        t.style.display = "none";
                    }
                }
                else {
                    btn.classList.remove("disabled");
                    for (let j = 0; j < tests.length; j++) {
                        const t = tests[j];
                        t.style.display = "";
                    }
                }
            };
        }
    }
    static updateRunPage(runGuid) {
        let run;
        this.loader.loadRunJson(runGuid, (response) => {
            run = JSON.parse(response, JsonLoader.reviveRun);
            if (run.name === "") {
                run.name = `${DateFormatter.format(run.runInfo.start)} - ${DateFormatter.format(run.runInfo.finish)}`;
            }
            UrlHelper.insertParam("runGuid", run.runInfo.guid);
            this.updateRunInformation(run);
            this.updateSummary(run);
            this.updateTitle(run);
            this.updateTestsList(run);
            this.updateTestFilterButtons();
            this.updateCopyright();
        });
        return run;
    }
    static updateTestsList(run) {
        const paths = new Array();
        var test;
        document.getElementById("btn-back").setAttribute("href", `./../index.html`);
        document.getElementById("all-tests").innerHTML = "";
        const files = run.testRunFiles;
        for (let i = 0; i < files.length; i++) {
            paths[i] = `./../tests/${files[i]}`;
        }
        var index = 0;
        this.loader.loadJsons(paths, 0, (response, c, i) => {
            test = JSON.parse(response, JsonLoader.reviveRun);
            this.addTest(test, c, i);
            if (i === c - 1)
                this.makeCollapsible();
            index++;
        });
    }
    static loadRun(index) {
        let runInfos;
        this.loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, JsonLoader.reviveRun);
            runInfos.sort(Sorter.itemInfoSorterByFinishDateFuncDesc);
            this.runsToShow = this.reportSettings.runsToDisplay >= 1 ? Math.min(runInfos.length, this.reportSettings.runsToDisplay) : runInfos.length;
            if (index === undefined || index.toString() === "NaN") {
                index = 0;
            }
            if (index === 0) {
                this.disableBtn("btn-next");
            }
            if (index === this.runsToShow - 1) {
                this.disableBtn("btn-prev");
            }
            this.currentRunIndex = index;
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
            runInfos.sort(Sorter.itemInfoSorterByFinishDateFuncDesc);
            this.runsToShow = this.reportSettings.runsToDisplay >= 1 ? Math.min(runInfos.length, this.reportSettings.runsToDisplay) : runInfos.length;
            const runInfo = runInfos.find((r) => r.guid === guid);
            if (runInfo != undefined) {
                this.enableBtns();
                let index = runInfos.indexOf(runInfo);
                if (index === 0) {
                    this.disableBtn("btn-next");
                }
                if (index >= this.runsToShow - 1) {
                    index = this.runsToShow - 1;
                    this.disableBtn("btn-prev");
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
        if (this.currentRunIndex === this.runsToShow - 1) {
            this.disableBtn("btn-prev");
            return;
        }
        else {
            this.enableBtns();
            this.currentRunIndex += 1;
            if (this.currentRunIndex >= this.runsToShow - 1) {
                this.currentRunIndex = this.runsToShow - 1;
                this.disableBtn("btn-prev");
            }
            this.loadRun(this.currentRunIndex);
        }
    }
    static loadNext() {
        if (this.currentRunIndex === 0) {
            this.disableBtn("btn-next");
            return;
        }
        else {
            this.enableBtns();
            this.currentRunIndex -= 1;
            if (this.currentRunIndex <= 0) {
                this.currentRunIndex = 0;
                this.disableBtn("btn-next");
            }
            this.loadRun(this.currentRunIndex);
        }
    }
    static loadLatest() {
        this.enableBtns();
        this.disableBtn("btn-next");
        this.loadRun(undefined);
    }
    static initializePage() {
        this.loader.loadReportSettingsJson((response) => {
            this.reportSettings = JSON.parse(response);
            const isLatest = UrlHelper.getParam("loadLatest");
            if (isLatest !== "true") {
                UrlHelper.removeParam("loadLatest");
                this.tryLoadRunByGuid();
            }
            else {
                UrlHelper.removeParam("loadLatest");
                this.loadLatest();
            }
        });
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
    static updateLatestRunInfo(latestRun) {
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(latestRun.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(latestRun.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${DateFormatter.diff(latestRun.runInfo.start, latestRun.runInfo.finish)}`;
    }
    static updateCopyright() {
        document.getElementById("copyright").innerHTML = `Copyright 2015 - 2017 © GhpReporter (version ${this.reportSettings.coreVersion})`;
    }
    static updateRunsList(runs) {
        let list = "";
        const c = runs.length;
        for (let i = 0; i < c; i++) {
            const r = runs[i];
            if (r.name === "") {
                r.name = `${DateFormatter.format(r.runInfo.start)} - ${DateFormatter.format(r.runInfo.finish)}`;
            }
            list += `<li id=$run-${r.runInfo.guid}>Run #${c - i - 1}: <a href="./runs/index.html?runGuid=${r.runInfo.guid}">${r.name}</a></li>`;
        }
        document.getElementById("all-runs").innerHTML = list;
    }
    static updateRunsInfo(runs, totalFiles) {
        document.getElementById("total").innerHTML = `<b>Loaded runs:</b> ${runs.length}`;
        document.getElementById("loaded").innerHTML = `<b>Total runs:</b> ${totalFiles}`;
    }
    static updatePlotlyBars(runs) {
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
    static updatePage() {
        let runInfos;
        const paths = new Array();
        const r = new Array();
        const runs = new Array();
        this.loader.loadRunsJson((response) => {
            runInfos = JSON.parse(response, JsonLoader.reviveRun);
            runInfos.sort(Sorter.itemInfoSorterByFinishDateFuncDesc);
            const runsToLoad = this.reportSettings.runsToDisplay >= 1 ? Math.min(this.reportSettings.runsToDisplay, runInfos.length) : runInfos.length;
            for (let i = 0; i < runsToLoad; i++) {
                paths[i] = `runs/run_${runInfos[i].guid}.json`;
            }
            this.loader.loadAllJsons(paths, 0, r, (responses) => {
                for (let i = 0; i < responses.length; i++) {
                    const loadedRun = JSON.parse(responses[i], JsonLoader.reviveRun);
                    if (loadedRun.name === "") {
                        loadedRun.name = `${DateFormatter.format(loadedRun.runInfo.start)} - ${DateFormatter.format(loadedRun.runInfo.finish)}`;
                    }
                    runs[i] = loadedRun;
                }
                const latestRun = runs[0];
                this.updateLatestRunInfo(latestRun);
                this.updatePlotlyBars(runs);
                this.updateRunsInfo(runs, runInfos.length);
                this.updateRunsList(runs);
                this.updateCopyright();
            });
        });
    }
    static initializePage() {
        this.loader.loadReportSettingsJson((response) => {
            this.reportSettings = JSON.parse(response);
            this.updatePage();
        });
        this.showTab("runs-stats", document.getElementById("tab-runs-stats"));
    }
    static showTab(idToShow, caller) {
        TabsHelper.showTab(idToShow, caller, this.reportPageTabsIds);
    }
}
ReportPageUpdater.loader = new JsonLoader(PageType.TestRunsPage);
ReportPageUpdater.reportPageTabsIds = ["runs-stats", "runs-list"];
class TestPageUpdater {
    static updateCopyright() {
        document.getElementById("copyright").innerHTML = `Copyright 2015 - 2017 © GhpReporter (version ${this.reportSettings.coreVersion})`;
    }
    static updateMainInformation(t) {
        document.getElementById("page-title").innerHTML = `<b>Test:</b> ${t.name}`;
        document.getElementById("name").innerHTML = `<b>Test name:</b> ${t.name}`;
        document.getElementById("full-name").innerHTML = `<b>Full name:</b> ${t.fullName}`;
        document.getElementById("description").innerHTML = `<b>Test description:</b> ${TestRunHelper.getDescription(t)}`;
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
        document.getElementById("test-output-string").innerHTML = `<b>Test log:</b><br>
		<div style="word-wrap: break-word;  white-space: pre-wrap;">${Differ.safeTagsReplace(TestRunHelper.getOutput(t))}</div>`;
    }
    static updateTestData(t) {
        let res = "";
        t.testData.forEach((td) => {
            res += `<li>${DateFormatter.format(td.date)}: ${td.comment} <br>${Differ.getHtml(td.actual, td.expected)}<br></li>`;
        });
        document.getElementById("test-data-list").innerHTML = `${res}`;
    }
    static updateScreenshots(t) {
        let screenshots = "";
        for (let i = 0; i < t.screenshots.length; i++) {
            const s = t.screenshots[i];
            const src = `./${t.testInfo.guid}/img/${s.name}`;
            screenshots += `<li><b>Screenshot ${DateFormatter.format(s.date)}:</b><a href="${src}"><img src="${src}" alt="${src}" style="width: 100%;"></img></a></li>`;
        }
        if (screenshots === "") {
            screenshots = "-";
        }
        document.getElementById("screenshots").innerHTML = screenshots;
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
        let t;
        this.loader.loadTestJson(testGuid, fileName, (response) => {
            t = JSON.parse(response, JsonLoader.reviveRun);
            UrlHelper.insertParam("testGuid", t.testInfo.guid);
            UrlHelper.insertParam("testFile", t.testInfo.fileName);
            this.updateMainInformation(t);
            this.updateOutput(t);
            this.updateFailure(t);
            this.updateScreenshots(t);
            this.updateTestData(t);
            document.getElementById("btn-back").setAttribute("href", `./../runs/index.html?runGuid=${t.runGuid}`);
            this.updateTestHistory();
            this.updateCopyright();
        });
        return t;
    }
    static updateTestHistory() {
        const paths = new Array();
        const testStrings = new Array();
        const tests = new Array();
        const guid = UrlHelper.getParam("testGuid");
        let testInfos;
        this.loader.loadTestsJson(guid, (response) => {
            testInfos = JSON.parse(response, JsonLoader.reviveRun);
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFuncDesc);
            for (let i = 0; i < this.testVersionsCount; i++) {
                paths[i] = `./${testInfos[i].guid}/${testInfos[i].fileName}`;
            }
            this.loader.loadAllJsons(paths, 0, testStrings, (responses) => {
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
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFuncDesc);
            this.testVersionsCount = this.reportSettings.testsToDisplay >= 1 ? Math.min(testInfos.length, this.reportSettings.testsToDisplay) : testInfos.length;
            if (index === undefined || index.toString() === "NaN") {
                index = 0;
            }
            if (index <= 0) {
                index = 0;
                this.disableBtn("btn-next");
            }
            else {
                this.enableBtn("btn-next");
            }
            if (index >= this.testVersionsCount - 1) {
                index = this.testVersionsCount - 1;
                this.disableBtn("btn-prev");
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
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFuncDesc);
            this.testVersionsCount = this.reportSettings.testsToDisplay >= 1 ? Math.min(testInfos.length, this.reportSettings.testsToDisplay) : testInfos.length;
            const testInfo = testInfos.find((t) => t.fileName === fileName);
            if (testInfo != undefined) {
                this.enableBtns();
                let index = testInfos.indexOf(testInfo);
                if (index <= 0) {
                    index = 0;
                    this.disableBtn("btn-next");
                }
                if (index >= this.testVersionsCount - 1) {
                    index = this.testVersionsCount - 1;
                    this.disableBtn("btn-prev");
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
    static enableBtn(id) {
        document.getElementById(id).removeAttribute("disabled");
    }
    static loadPrev() {
        if (this.currentTest === this.testVersionsCount - 1) {
            this.disableBtn("btn-prev");
            return;
        }
        else {
            this.enableBtns();
            this.currentTest += 1;
            if (this.currentTest >= this.testVersionsCount - 1) {
                this.currentTest = this.testVersionsCount - 1;
                this.disableBtn("btn-prev");
            }
            this.loadTest(this.currentTest);
        }
    }
    static loadNext() {
        if (this.currentTest === 0) {
            this.disableBtn("btn-next");
            return;
        }
        else {
            this.enableBtns();
            this.currentTest -= 1;
            if (this.currentTest <= 0) {
                this.currentTest = 0;
                this.disableBtn("btn-next");
            }
            this.loadTest(this.currentTest);
        }
    }
    static loadLatest() {
        this.enableBtns();
        this.disableBtn("btn-next");
        this.loadTest(undefined);
    }
    static initializePage() {
        this.loader.loadReportSettingsJson((response) => {
            this.reportSettings = JSON.parse(response);
            const isLatest = UrlHelper.getParam("loadLatest");
            if (isLatest !== "true") {
                UrlHelper.removeParam("loadLatest");
                this.tryLoadTestByGuid();
            }
            else {
                UrlHelper.removeParam("loadLatest");
                this.loadLatest();
            }
        });
        const tabFromUrl = UrlHelper.getParam("currentTab");
        const tab = tabFromUrl === "" ? "test-history" : tabFromUrl;
        this.showTab(tab === "" ? "test-history" : tab, document.getElementById(`tab-${tab}`));
    }
    static showTab(idToShow, caller) {
        TabsHelper.showTab(idToShow, caller, this.runPageTabsIds);
    }
}
TestPageUpdater.loader = new JsonLoader(PageType.TestPage);
TestPageUpdater.runPageTabsIds = ["test-history", "test-output", "test-failure", "test-screenshots", "test-data"];
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
    static itemInfoSorterByFinishDateFuncDesc(a, b) {
        if (a.finish < b.finish) {
            return 1;
        }
        if (a.finish > b.finish) {
            return -1;
        }
        return 0;
    }
}
function loadRun1(guid) {
}
//# sourceMappingURL=ghpr.controller.js.map