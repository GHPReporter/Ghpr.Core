///<reference path="./localFileSystem/entities/ItemInfo.ts"/>
///<reference path="./localFileSystem/entities/ReportSettings.ts"/>
///<reference path="./localFileSystem/entities/Run.ts"/>
///<reference path="./localFileSystem/entities/TestRun.ts"/>
///<reference path="./localFileSystem/entities/TestData.ts"/>
///<reference path="./../enums/PageType.ts"/>
///<reference path="./JsonParser.ts"/>
///<reference path="./UrlHelper.ts"/>
///<reference path="./DateFormatter.ts"/>
///<reference path="./Color.ts"/>
///<reference path="./PlotlyJs.ts"/>
///<reference path="./Differ.ts"/>
///<reference path="./TabsHelper.ts"/>
///<reference path="./TestRunHelper.ts"/>
///<reference path="./../Controller.ts"/>
///<reference path="./../dto/ReportSettingsDto.ts"/>
///<reference path="./../dto/TestOutputDto.ts"/>
///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../interfaces/IDataService.ts"/>

class TestPageUpdater {

    static currentTest: number;
    static testVersionsCount: number;
    static reviveRun = JsonParser.reviveRun;
        
    private static updateCopyright(coreVersion: string): void {
        document.getElementById("copyright").innerHTML = `Copyright 2015 - 2018 © GhpReporter (version ${coreVersion})`;
    }

    private static updateMainInformation(t: TestRunDto): void {
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

    private static updateOutput(t: TestRunDto): void {
        Controller.dataService.fromPage(PageType.TestPage).getTestOutput(t, (to: TestOutputDto) => {
            let o = Differ.safeTagsReplace(TestRunHelper.getOutput(to));
            let eo = Differ.safeTagsReplace(TestRunHelper.getExtraOutput(to));
            document.getElementById("test-output-string").innerHTML = `<b>Test log:</b><br>
    		<div style="word-wrap: break-word;  white-space: pre-wrap;">${o}</div>`;
            document.getElementById("test-extra-output-string").innerHTML = `<b>Additional log:</b><br>
    		<div style="word-wrap: break-word;  white-space: pre-wrap;">${eo}</div>`;
        });
    }

    private static updateTestData(t: TestRunDto): void {
        let res = "";
        t.testData.forEach((td: TestDataDto) => {
            res += `<li>${DateFormatter.format(td.testDataInfo.date)}: ${td.comment} <br>${Differ.getHtml(td.actual, td.expected)}<br></li>`;
        });
        if (res === "") {
            res = "-";
        }
        document.getElementById("test-data-list").innerHTML = `${res}`;
    }

    private static updateScreenshots(t: TestRunDto): void {
        Controller.dataService.fromPage(PageType.TestPage).getTestScreenshots(t, (screenshotDtos: Array<TestScreenshotDto>) => {
            let screenshots = "";
            for (let i = 0; i < screenshotDtos.length; i++) {
                const s = screenshotDtos[i];
                const src = `data:image/${s.format};base64, ${s.base64Data}`;
                const date = DateFormatter.format(s.testScreenshotInfo.date);
                const alt = s.testScreenshotInfo.itemName;
                screenshots += `<li><b>Screenshot ${date}:</b><a href="${src}" target="_blank"><img src="${src}" alt="${alt}" style="width: 100%;"></img></a></li>`;
            }
            if (screenshots === "") {
                screenshots = "-";
            }
            document.getElementById("screenshots").innerHTML = screenshots;
        });
    }

    private static updateFailure(t: TestRunDto): void {
        document.getElementById("test-message").innerHTML = `<b>Message:</b><br> ${TestRunHelper.getMessage(t)}`;
        document.getElementById("test-stack-trace").innerHTML = `<b>Stack trace:</b><br> ${TestRunHelper.getStackTrace(t)}`;
    }

    private static setTestHistory(tests: Array<TestRunDto>): void {
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

    private static updateTestPage(testGuid: string, itemName: string): void {
        Controller.dataService.fromPage(PageType.TestPage).getLatestTest(testGuid, itemName, (t: TestRunDto) => {
            UrlHelper.insertParam("testGuid", t.testInfo.guid);
            UrlHelper.insertParam("itemName", t.testInfo.itemName);
            this.updateMainInformation(t);
            this.updateOutput(t);
            this.updateFailure(t);
            this.updateScreenshots(t);
            this.updateTestData(t);
            document.getElementById("btn-back").setAttribute("href", `./../runs/index.html?runGuid=${t.runGuid}`);
            this.updateTestHistory();
            this.updateCopyright(Controller.reportSettings.coreVersion);
        });
    }

    static updateTestHistory(): void {
        const guid = UrlHelper.getParam("testGuid");
        Controller.dataService.fromPage(PageType.TestPage).getLatestTests(guid, (testRunDtos: Array<TestRunDto>, total: number) => {
            this.setTestHistory(testRunDtos);
        });
    }

    private static loadTest(index: number): void {
        const guid = UrlHelper.getParam("testGuid");
        Controller.dataService.fromPage(PageType.TestPage).getTestInfos(guid, (testInfoDtos: ItemInfoDto[]) => {
            let testsToDisplay = Controller.reportSettings.testsToDisplay;
            this.testVersionsCount = testsToDisplay >= 1 ? Math.min(testInfoDtos.length, testsToDisplay) : testInfoDtos.length;
            if (index === undefined || index.toString() === "NaN") {
                index = 0;
            }
            if (index <= 0) {
                index = 0;
                this.disableBtn("btn-next");
            } else {
                this.enableBtn("btn-next");
            }
            if (index >= this.testVersionsCount - 1) {
                index = this.testVersionsCount - 1;
                this.disableBtn("btn-prev");
            }
            this.currentTest = index;
            this.updateTestPage(testInfoDtos[index].guid, testInfoDtos[index].itemName);
        });
    }

    private static tryLoadTestByGuid(): void {
        const guid = UrlHelper.getParam("testGuid");
        const itemName = UrlHelper.getParam("itemName");
        Controller.dataService.fromPage(PageType.TestPage).getTestInfos(guid, (testInfoDtos: ItemInfoDto[]) => {
            let testsToDisplay = Controller.reportSettings.testsToDisplay;
            this.testVersionsCount = testsToDisplay >= 1 ? Math.min(testInfoDtos.length, testsToDisplay) : testInfoDtos.length;
            const testInfo = testInfoDtos.find((t) => t.itemName === itemName);
            if (testInfo != undefined) {
                this.enableBtns();
                let index = testInfoDtos.indexOf(testInfo);
                if (index <= 0) {
                    index = 0;
                    this.disableBtn("btn-next");
                }
                if (index >= this.testVersionsCount - 1) {
                    index = this.testVersionsCount - 1;
                    this.disableBtn("btn-prev");
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

    private static enableBtn(id: string): void {
        document.getElementById(id).removeAttribute("disabled");
    }

    static loadPrev(): void {
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

    static loadNext(): void {
        if (this.currentTest === 0) {
            this.disableBtn("btn-next");
            return;
        } else {
            this.enableBtns();
            this.currentTest -= 1;
            if (this.currentTest <= 0) {
                this.currentTest = 0;
                this.disableBtn("btn-next");
            }
            this.loadTest(this.currentTest);
        }
    }

    static loadLatest(): void {
        this.enableBtns();
        this.disableBtn("btn-next");
        this.loadTest(undefined);
    }

    static initializePage(): void {
        Controller.init(PageType.TestPage, (dataService: IDataService, reportSettings: ReportSettingsDto) => {
            const isLatest = UrlHelper.getParam("loadLatest");
            if (isLatest !== "true") {
                UrlHelper.removeParam("loadLatest");
                this.tryLoadTestByGuid();
            } else {
                UrlHelper.removeParam("loadLatest");
                this.loadLatest();
            }
        });
        const tabFromUrl = UrlHelper.getParam("currentTab");
        const tab = tabFromUrl === "" ? "test-history" : tabFromUrl;
        this.showTab(tab === "" ? "test-history" : tab, document.getElementById(`tab-${tab}`));
    }

    private static runPageTabsIds: Array<string> = ["test-history", "test-output", "test-extra-output", "test-failure", "test-screenshots", "test-data"];

    static showTab(idToShow: string, caller: HTMLElement): void {
        TabsHelper.showTab(idToShow, caller, this.runPageTabsIds);
    }
}