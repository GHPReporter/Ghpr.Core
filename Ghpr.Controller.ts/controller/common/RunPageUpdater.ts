///<reference path="./localFileSystem/entities/ReportSettings.ts"/>
///<reference path="./RunPagePlotly.ts"/>
///<reference path="./TestRunHelper.ts"/>
///<reference path="./../Controller.ts"/>
///<reference path="./../common/DocumentHelper.ts"/>

class RunPageUpdater {

    static currentRunIndex: number;
    static runsToShow: number;
    static reviveRun = JsonParser.reviveRun;

    private static updateRunInformation(run: RunDto): void {
        document.getElementById("name").innerHTML = `<b>Name:</b> ${run.name}`;
        document.getElementById("sprint").innerHTML = `<b>Sprint:</b> ${run.sprint}`;
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(run.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(run.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${DateFormatter.diff(run.runInfo.start, run.runInfo.finish)}`;
    }
    
    private static updateBriefResults(run: RunDto): void {
        const s = run.summary;
        document.getElementById("run-results").innerHTML = `<div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Total</a>
            <p class="f6 text-gray mb-2">${s.total}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Success</a>
            <p class="f6 text-gray mb-2">${s.success}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Errors</a>
            <p class="f6 text-gray mb-2">${s.errors}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Failures</a>
            <p class="f6 text-gray mb-2">${s.failures}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Inconclusive</a>
            <p class="f6 text-gray mb-2">${s.inconclusive}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Ignored</a>
            <p class="f6 text-gray mb-2">${s.ignored}</p>
            </div></div><div class="mx-4 py-2 border-bottom"><div>
            <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">Unknown</a>
            <p class="f6 text-gray mb-2">${s.unknown}</p>
            </div></div>`;
    }

    private static addTest(t: TestRunDto, c: number, i: number): void {
        const ti = t.testInfo;
        const color = TestRunHelper.getColor(t);
        const result = TestRunHelper.getResult(t);
        const testHref = `./../tests/index.html?testGuid=${ti.guid}&itemName=${t.testInfo.itemName}`;
        const testLi = `<li id="test-${ti.guid}" class="${result} ghpr-test" style="color: white;">
            <span class="ghpr-test-list-span" style="background-color: ${color};"></span>
            <a href="${testHref}"> ${t.name}</a></li>`;
        const failedTestLi = `<li><div class="width-full text-bold">
                                <span class="ghpr-test-list-span" style="background-color: ${color};"></span>
                                <a class="f5 mb-2" href="${testHref}"> ${t.name}</a>
                              </div></li>`;
        if (result === TestResult.Failed) {
            document.getElementById("recent-test-failures").innerHTML += failedTestLi;
        }
        //getting correct namespace to build hierarchical test list
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
        //remove all special symbols:
        for (let j = len1 - 1; j >= 0; j -= 1) {
            if (/[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(arr[j])) {
                arr.splice(j, 1);
            }
        }
        //remove name from fullName if it was not removed:
        let len2 = arr.length;
        if ((len1 === len2) && !nameRemoved) {
            arr.splice(len2 - 1, 1);
            len2 = arr.length;
        }
        const ids: string[] = new Array();
        for (let j = 0; j < len2; j++) {
            ids[j] = `hierarchical-${arr.slice(0, j + 1).join(".").replace(/\s/g, "_")}`;
        }
        const hierarchicalListElement = document.getElementById("all-tests-hierarchical");
        for (let j = 0; j <= len2; j++) {
            const el = hierarchicalListElement.querySelectorAll(`li[id="${ids[j]}"]`)[0];
            if (el === null || el === undefined) {
                const li = `<li id="${ids[j]}" class="test-suite"><a>${arr[j]}</a><ul></ul></li>`;
                if (j === 0) {
                    hierarchicalListElement.innerHTML += li;
                } else {
                    const firstUl = hierarchicalListElement.querySelector(`li[id="${ids[j - 1]}"]`).getElementsByTagName("ul")[0];
                    if (j !== len2) {
                        firstUl.innerHTML += li;
                    } else {
                        firstUl.innerHTML += testLi;
                    }
                }
            }
        }
        const namespace = `${arr.join(".").replace(/\s/g, "_")}`;
        const collapsedListElement = document.getElementById("all-tests-collapsed");
        const collapsedId = `collapsed-${namespace}`;
        const el = collapsedListElement.querySelectorAll(`li[id="${collapsedId}"]`)[0];
        if (el === null || el === undefined) {
            collapsedListElement.innerHTML +=
                `<li id="${collapsedId}" class="test-suite" style="list-style: none;"><a>${namespace}</a><ul></ul></li>`;
        }
        const li = collapsedListElement.querySelector(`li[id="${collapsedId}"]`).getElementsByTagName("ul")[0];
        li.innerHTML += testLi;
        //make sure all tests are displayed with respect to filter buttons:
        const btns = document.getElementById("test-result-filter-buttons").getElementsByTagName("button");
        for (let i = 0; i < btns.length; i++) {
            const btn = btns[i];
            const id = btn.getAttribute("id");
            const tests = document.getElementsByClassName(id);
            btn.style.backgroundImage = "none";
            btn.style.backgroundColor = TestRunHelper.getColorByResult(Number(id));
            for (let j = 0; j < tests.length; j++) {
                const t = tests[j] as HTMLElement;
                if (!btn.classList.contains("disabled")) {
                    t.style.display = "";
                } else {
                    t.style.display = "none";
                }
            }
        }
    }

    public static toHierarchicalList(): void {
        const collapsedList = document.getElementById("all-tests-collapsed");
        const hierarchicalList = document.getElementById("all-tests-hierarchical");
        collapsedList.style.display = "none";
        hierarchicalList.style.display = "";
    }

    public static toCollapsedList(): void {
        const collapsedList = document.getElementById("all-tests-collapsed");
        const hierarchicalList = document.getElementById("all-tests-hierarchical");
        collapsedList.style.display = "";
        hierarchicalList.style.display = "none";
    }

    private static makeCollapsible(): void {
        const targets = document.getElementsByClassName("test-suite");
        for (let i = 0; i < targets.length; i++) {
            const t = targets[i];
            t.getElementsByTagName("a")[0].onclick = () => {
                const e = t.getElementsByTagName("ul")[0];
                if (e.style.display === "") {
                    e.style.display = "none";
                } else {
                    e.style.display = "";
                }
            };
        }
    }

    private static updateTestFilterButtons(): void {
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
                        const t = tests[j] as HTMLElement;
                        t.style.display = "none";
                    }
                    const lis = document.getElementsByClassName("test-suite");
                    for (let j = 0; j < lis.length; j++) {
                        const li = lis[j] as HTMLElement;
                        const liTests = li.getElementsByClassName("ghpr-test");
                        let anyTestsDisplayed = false;
                        for (let k = 0; k < liTests.length; k++) {
                            const liTest = liTests[k] as HTMLElement;
                            if (liTest.style.display !== "none") {
                                anyTestsDisplayed = true;
                            }
                        }
                        if (!anyTestsDisplayed) {
                            li.style.display = "none";
                        }
                    }
                } else {
                    btn.classList.remove("disabled");
                    for (let j = 0; j < tests.length; j++) {
                        const t = tests[j] as HTMLElement;
                        t.style.display = "";
                    }
                    const lis = document.getElementsByClassName("test-suite");
                    for (let j = 0; j < lis.length; j++) {
                        const li = lis[j] as HTMLElement;
                        const liTests = li.getElementsByClassName("ghpr-test");
                        let anyTestsDisplayed = false;
                        for (let k = 0; k < liTests.length; k++) {
                            const liTest = liTests[k] as HTMLElement;
                            if (liTest.style.display !== "none") {
                                anyTestsDisplayed = true;
                            }
                        }
                        if (anyTestsDisplayed) {
                            li.style.display = "";
                        }
                    }
                }
            };
        }
    }

    private static updateRunPage(runGuid: string): void {
        Controller.init(PageType.TestRunPage, (dataService: IDataService, reportSettings: ReportSettingsDto) => {
            dataService.fromPage(PageType.TestRunPage).getRun(runGuid, (runDto: RunDto) => {
                UrlHelper.insertParam("runGuid", runDto.runInfo.guid);
                RunPagePlotly.resetTimelineData();
                DocumentHelper.updateReportName(reportSettings.reportName);
                this.updateRunInformation(runDto);
                RunPagePlotly.updateSummary(runDto, "summary-pie");
                this.updateBriefResults(runDto);
                DocumentHelper.setInnerHtmlById("page-title", runDto.name);
                this.updateTestFilterButtons();
                this.updateTestsList(runDto);
                RunPagePlotly.updateTimeline("run-timeline-chart");
                DocumentHelper.updateCopyright(reportSettings.coreVersion);
                window.addEventListener("resize", () => {
                    RunPagePlotly.relayoutSummaryPlot("summary-pie");
                    RunPagePlotly.relayoutTimelinePlot("run-timeline-chart");
                });
            });
        });
    }

    static updateTestsList(run: RunDto): void {
        document.getElementById("btn-back").setAttribute("href", `./../index.html`);
        document.getElementById("all-tests-hierarchical").innerHTML = "";
        document.getElementById("recent-test-failures").innerHTML = "";
        var index = 0;
        Controller.init(PageType.TestRunPage, (dataService: IDataService, reportSettings: ReportSettingsDto) => {
            dataService.fromPage(PageType.TestRunPage).getRunTests(run, (testRunDto: TestRunDto, c: number, i: number) => {
                this.addTest(testRunDto, c, i);
                RunPagePlotly.addTestRunDto(testRunDto);
                if (i === c - 1) {
                    this.makeCollapsible();
                    RunPagePlotly.updateTimeline("run-timeline-chart");
                }
                index++;
            });
        });
    }
    
    private static loadRun(index: number): void {
        Controller.dataService.fromPage(PageType.TestRunPage).getRunInfos((runInfoDtos: ItemInfoDto[]) => {
            let runsToDisplay = Controller.reportSettings.runsToDisplay;
            this.runsToShow = runsToDisplay >= 1 ? Math.min(runInfoDtos.length, runsToDisplay) : runInfoDtos.length;
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
            this.updateRunPage(runInfoDtos[index].guid);
        });
    }

    private static tryLoadRunByGuid(): void {
        const guid = UrlHelper.getParam("runGuid");
        if (guid === "") {
            this.loadRun(undefined);
            return;
        }
        Controller.dataService.fromPage(PageType.TestRunPage).getRunInfos((runInfoDtos: ItemInfoDto[]) => {
            let runsToDisplay = Controller.reportSettings.runsToDisplay;
            this.runsToShow = runsToDisplay >= 1
                    ? Math.min(runInfoDtos.length, runsToDisplay) : runInfoDtos.length;
                const runInfo = runInfoDtos.find((r) => r.guid === guid);
                if (runInfo != undefined) {
                    this.enableBtns();
                    let index = runInfoDtos.indexOf(runInfo);
                    if (index === 0) {
                        this.disableBtn("btn-next");
                    }
                    if (index >= this.runsToShow - 1) {
                        index = this.runsToShow - 1;
                        this.disableBtn("btn-prev");
                    }
                    this.loadRun(index);
                } else {
                    this.loadRun(undefined);
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

    static loadNext(): void {
        if (this.currentRunIndex === 0) {
            this.disableBtn("btn-next");
            return;
        } else {
            this.enableBtns();
            this.currentRunIndex -= 1;
            if (this.currentRunIndex <= 0) {
                this.currentRunIndex = 0;
                this.disableBtn("btn-next");
            }
            this.loadRun(this.currentRunIndex);
        }
    }

    static loadLatest(): void {
        this.enableBtns();
        this.disableBtn("btn-next");
        this.loadRun(undefined);
    }

    static initializePage(): void {
        Controller.init(PageType.TestRunPage, (dateService: IDataService, reportSettings: ReportSettingsDto) => {
            const isLatest = UrlHelper.getParam("loadLatest");
            if (isLatest !== "true") {
                UrlHelper.removeParam("loadLatest");
                this.tryLoadRunByGuid();
            } else {
                UrlHelper.removeParam("loadLatest");
                this.loadLatest();
            }
        });
        const tabFromUrl = UrlHelper.getParam("currentTab");
        const tab = tabFromUrl === "" ? "run-main-stats" : tabFromUrl;
        this.showTab(tab, document.getElementById(`tab-${tab}`));
    }

    private static runPageTabsIds: Array<string> = ["run-main-stats", "run-test-list", "run-timeline"];

    static showTab(idToShow: string, caller: HTMLElement): void {
        TabsHelper.showTab(idToShow, caller, this.runPageTabsIds);
    }
}