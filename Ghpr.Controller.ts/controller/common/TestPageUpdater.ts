///<reference path="./localFileSystem/entities/ReportSettings.ts"/>
///<reference path="./PlotlyJs.ts"/>
///<reference path="./Differ.ts"/>
///<reference path="./../Controller.ts"/>
///<reference path="./../common/DocumentHelper.ts"/>

class TestPageUpdater {

    static currentTest: number;
    static testVersionsCount: number;
    static reviveRun = JsonParser.reviveRun;

    private static updateRecentData(t: TestRunDto): void {
        document.getElementById("test-results").innerHTML = `<div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Start datetime</a>
            <p class="f6 text-gray mb-2">${DateFormatter.format(t.testInfo.start)}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Finish datetime</a>
            <p class="f6 text-gray mb-2">${DateFormatter.format(t.testInfo.finish)}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Duration</a>
            <p class="f6 text-gray mb-2">${t.duration.toString()}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Categories</a>
            <p class="f6 text-gray mb-2">${TestRunHelper.getCategories(t)}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Test type</a>
            <p class="f6 text-gray mb-2">${TestRunHelper.getTestType(t)}</p>
            </div></div>`;
    }

    private static updateMainInformation(t: TestRunDto): void {
        document.getElementById("page-title").innerHTML = `<b>Test:</b> ${t.name}`;
        document.getElementById("name").innerHTML = `<b>Test name:</b> ${t.name}`;
        document.getElementById("full-name").innerHTML = `<b>Full name:</b> ${t.fullName}`;
        document.getElementById("description").innerHTML = `<b>Test description:</b> ${TestRunHelper.getDescription(t)}`;
        document.getElementById("result").innerHTML = `<b>Result:</b> ${TestRunHelper.getColoredResult(t)}`;
        document.getElementById("priority").innerHTML = `<b>Priority:</b> ${TestRunHelper.getPriority(t)}`;
        document.getElementById("test-type").innerHTML = `<b>Test type:</b> ${TestRunHelper.getTestType(t)}`;
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
        document.getElementById("test-message").innerHTML = `<b>Message:</b><br>${TestRunHelper.getMessage(t)}`;
        document.getElementById("test-stack-trace").innerHTML = `<b>Stack trace:</b><br><code style="white-space: pre-wrap">${TestRunHelper.getStackTrace(t)}</code>`;
    }

    static getTestHistoryPlotSize(plotDiv: HTMLElement): any {
        const p = plotDiv.parentElement.parentElement.parentElement;
        const w = Math.max(300, Math.min(p.offsetWidth, 1100));
        const h = Math.max(300, Math.min(p.offsetHeight, 500));
        return { width: 0.95 * w, height: 0.95 * h };
    }

    private static setTestRecentFailures(tests: Array<TestRunDto>): void {
        const recentFailuresDiv = document.getElementById("recent-test-failures");
        recentFailuresDiv.innerHTML = "";
        const c = tests.length;
        for (let i = 0; i < c; i++) {
            const t = tests[i];
            const res = TestRunHelper.getResult(t);
            if (res === TestResult.Failed) {
                const ti = t.testInfo;
                const testPeriod = `${DateFormatter.format(t.testInfo.start)} - ${DateFormatter.format(t.testInfo.finish)}`;
                const href = `index.html?testGuid=${ti.guid}&itemName=${ti.itemName}&currentTab=test-history`;
                recentFailuresDiv.innerHTML += `<li><div class="width-full text-bold">
                                <span class="ghpr-test-list-span" style="background-color: ${Color.failed};"></span>
                                <a class="f5 p-1 mb-2" href="${href}">${testPeriod}</a>
                              </div></li>`;
            }
        }
    }

    private static setTestHistory(tests: Array<TestRunDto>): void {
        const historyDiv = document.getElementById("test-history-chart");
        let plotlyData = new Array();
        const dataX: Array<Date> = new Array();
        const dataY: Array<number> = new Array();
        const urls: Array<string> = new Array();
        const tickvals: Array<number> = new Array();
        const ticktext: Array<string> = new Array();
        const colors: Array<string> = new Array();
        const c = tests.length;
        for (let i = 0; i < c; i++) {
            const t = tests[i];
            const ti = t.testInfo;
            dataX[i] = ti.finish;
            colors[i] = TestRunHelper.getColor(t);
            const j = c - i - 1;
            dataY[i] = t.duration;
            tickvals[i] = j;
            ticktext[i] = `test ${j}`;
            urls[i] = `index.html?testGuid=${ti.guid}&itemName=${ti.itemName}&currentTab=test-history`; //t.testInfo.itemName;
        }
        const historyTrace = {
            x: dataX,
            y: dataY,
            customdata: urls,
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
            customdata: [urls[index]],
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

        var size = this.getTestHistoryPlotSize(historyDiv);

        const layout = {
            title: "Test history",
            xaxis: {
                title: "Finish datetime"
            },
            yaxis: {
                title: "Test duration (sec.)"
            },
            width: size.width,
            height: size.height
        };
        
        Plotly.newPlot(historyDiv, plotlyData, layout);

        (historyDiv as any).on("plotly_click", (eventData: any) => {
            var url = `${eventData.points[0].customdata}`;
            window.open(url, "_self");
        });
    }

    private static updateTestPage(testGuid: string, itemName: string): void {
        Controller.dataService.fromPage(PageType.TestPage).getLatestTest(testGuid, itemName, (t: TestRunDto) => {
            Controller.dataService.fromPage(PageType.TestPage).getRun(t.runGuid, (runDto: RunDto) => {
                var currentTestInRun = runDto.testsInfo.filter(ti => ti.guid === t.testInfo.guid)[0];
                var ind = runDto.testsInfo.indexOf(currentTestInRun);
                var prevTi = runDto.testsInfo[Math.max(ind - 1, 0)];
                var nextTi = runDto.testsInfo[Math.min(ind + 1, runDto.testsInfo.length - 1)];
                document.getElementById("btn-prev-from-run")
                    .setAttribute("href", `index.html?testGuid=${prevTi.guid}&itemName=${prevTi.itemName}&currentTab=test-history`);
                document.getElementById("btn-next-from-run")
                    .setAttribute("href", `index.html?testGuid=${nextTi.guid}&itemName=${nextTi.itemName}&currentTab=test-history`);
            });
            UrlHelper.insertParam("testGuid", t.testInfo.guid);
            UrlHelper.insertParam("itemName", t.testInfo.itemName);
            DocumentHelper.updateReportName(Controller.reportSettings.reportName);
            this.updateMainInformation(t);
            this.updateRecentData(t);
            this.updateOutput(t);
            this.updateFailure(t);
            this.updateScreenshots(t);
            this.updateTestData(t);
            document.getElementById("btn-back").setAttribute("href", `./../runs/index.html?runGuid=${t.runGuid}`);
            this.updateTestHistory();
            DocumentHelper.updateCopyright(Controller.reportSettings.coreVersion);

            window.addEventListener("resize", () => {
                const historyDiv = document.getElementById("test-history-chart");
                var size = this.getTestHistoryPlotSize(historyDiv);
                Plotly.relayout(historyDiv, { width: size.width, height: size.height });
            });
        });
    }

    static updateTestHistory(): void {
        const guid = UrlHelper.getParam("testGuid");
        Controller.dataService.fromPage(PageType.TestPage).getLatestTests(guid, (testRunDtos: Array<TestRunDto>, total: number) => {
            this.setTestHistory(testRunDtos);
            this.setTestRecentFailures(testRunDtos);
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