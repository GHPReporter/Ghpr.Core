///<reference path="./../interfaces/IItemInfo.ts"/>
///<reference path="./../interfaces/IRun.ts"/>
///<reference path="./../interfaces/ITestRun.ts"/>
///<reference path="./../enums/PageType.ts"/>
///<reference path="./JsonLoader.ts"/>
///<reference path="./UrlHelper.ts"/>
///<reference path="./DateFormatter.ts"/>
///<reference path="./Color.ts"/>
///<reference path="./PlotlyJs.ts"/>
///<reference path="./TabsHelper.ts"/>
///<reference path="./TestRunHelper.ts"/>

class TestPageUpdater {

    static currentTest: number;
    static testVersionsCount: number;
    static loader = new JsonLoader(PageType.TestPage);

    private static updateMainInformation(t: ITestRun): void {
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

    private static updateOutput(t: ITestRun): void {
        document.getElementById("test-output-string").innerHTML = `<b>Test log:</b><br> ${TestRunHelper.getOutput(t)}`;
    }

    private static updateScreenshots(t: ITestRun): void {
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

    private static updateFailure(t: ITestRun): void {
        document.getElementById("test-message").innerHTML = `<b>Message:</b><br> ${TestRunHelper.getMessage(t)}`;
        document.getElementById("test-stack-trace").innerHTML = `<b>Stack trace:</b><br> ${TestRunHelper.getStackTrace(t)}`;
    }

    private static setTestHistory(tests: Array<ITestRun>): void {
        const historyDiv = document.getElementById("test-history-chart");
        let plotlyData = new Array();
        const dataX: Array<Date> = new Array();
        const dataY: Array<number> = new Array();
        const tickvals: Array<number> = new Array();
        const ticktext: Array<string> = new Array();
        const colors: Array<string> = new Array();

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

    private static updateTestPage(testGuid: string, fileName: string): ITestRun {
        let t: ITestRun;
        this.loader.loadTestJson(testGuid, fileName, (response: string) => {
            t = JSON.parse(response, JsonLoader.reviveRun);
            UrlHelper.insertParam("testGuid", t.testInfo.guid);
            UrlHelper.insertParam("testFile", t.testInfo.fileName);
            this.updateMainInformation(t);
            this.updateOutput(t);
            this.updateFailure(t);
            this.updateScreenshots(t);
            document.getElementById("btn-back").setAttribute("href", `./../runs/index.html?runGuid=${t.runGuid}`);
            this.updateTestHistory();
        });
        return t;
    }

    static updateTestHistory(): void {
        const paths: Array<string> = new Array();
        const testStrings: Array<string> = new Array();
        const tests: Array<ITestRun> = new Array();
        const guid = UrlHelper.getParam("testGuid");
        let testInfos: Array<IItemInfo>;
        this.loader.loadTestsJson(guid, (response: string) => {
            testInfos = JSON.parse(response, JsonLoader.reviveRun);
            testInfos.sort(Sorter.itemInfoSorterByFinishDateFunc);

            for (let i = 0; i < testInfos.length; i++) {
                paths[i] = `./${testInfos[i].guid}/${testInfos[i].fileName}`;
            }

            this.loader.loadAllJsons(paths, 0, testStrings, (responses: Array<string>) => {
                for (let i = 0; i < responses.length; i++) {
                    tests[i] = JSON.parse(responses[i], JsonLoader.reviveRun);
                }
                this.setTestHistory(tests);
            });
        });
        
    }

    private static loadTest(index: number): void {
        const guid = UrlHelper.getParam("testGuid");
        let testInfos: Array<IItemInfo>;
        this.loader.loadTestsJson(guid, (response: string) => {
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

    private static tryLoadTestByGuid(): void {
        const guid = UrlHelper.getParam("testGuid");
        const fileName = UrlHelper.getParam("testFile");
        let testInfos: Array<IItemInfo>;
        this.loader.loadTestsJson(guid, (response: string) => {
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
            } else {
                this.loadTest(undefined);
            }
        });
    }

    private static enableBtns(): void {
        document.getElementById("btn-prev").removeAttribute("disabled");
        document.getElementById("btn-next").removeAttribute("disabled");
    }

    private static disableBtn(id: string): void {
        document.getElementById(id).setAttribute("disabled", "true");
    }

    static loadPrev(): void {
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

    static loadNext(): void {
        if (this.currentTest === this.testVersionsCount - 1) {
            this.disableBtn("btn-next");
            return;
        } else {
            this.enableBtns();
            this.currentTest += 1;
            if (this.currentTest === this.testVersionsCount - 1) {
                this.disableBtn("btn-next");
            }
            this.loadTest(this.currentTest);
        }
    }

    static loadLatest(): void {
        this.disableBtn("btn-next");
        this.loadTest(undefined);
    }

    static initializePage(): void {
        this.tryLoadTestByGuid();
        const tabFromUrl = UrlHelper.getParam("currentTab");
        const tab = tabFromUrl === "" ? "test-history" : tabFromUrl;
        this.showTab(tab === "" ? "test-history" : tab, document.getElementById(`tab-${tab}`));
    }

    private static runPageTabsIds: Array<string> = ["test-history", "test-output", "test-failure", "test-screenshots"];

    static showTab(idToShow: string, caller: HTMLElement): void {
        TabsHelper.showTab(idToShow, caller, this.runPageTabsIds);
    }
}