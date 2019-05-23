///<reference path="./localFileSystem/entities/ReportSettings.ts"/>
///<reference path="./ReportPagePlotly.ts"/>
///<reference path="./../Controller.ts"/>
///<reference path="./../common/DocumentHelper.ts"/>

class ReportPageUpdater {

    private static updateLatestRunInfo(latestRun: RunDto): void {
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(latestRun.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(latestRun.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${DateFormatter.diff(latestRun.runInfo.start, latestRun.runInfo.finish)}`;
    }

    private static updateRunsList(runs: Array<RunDto>): void {
        let fullList = "";
        let recentList = "";
        let runsResultsList = "";
        const c = runs.length;
        for (let i = 0; i < c; i++) {
            const r = runs[i];
            if (r.name === "") {
                r.name = `${DateFormatter.format(r.runInfo.start)} - ${DateFormatter.format(r.runInfo.finish)}`;
            }
            fullList += `<li id=$run-${r.runInfo.guid}>Run #${c - i - 1}: <a href="./runs/index.html?runGuid=${r.runInfo.guid}">${r.name}</a></li>`;
            recentList += `<li id=$run-${r.runInfo.guid}><div class="width-full text-bold">
                            <a class="d-flex flex-items-baseline f5 mb-2" href="./runs/index.html?runGuid=${r.runInfo.guid}">${r.name}</a>
                            </div></li>`;
            const bb = i === c - 1 ? "" : "border-bottom";
            let passed = r.summary.success === r.summary.total;
            const status = passed ? "All tests passed" : "Some errors detected";
            const statusIconPath = passed ? "./src/octicons/check.svg" : "./src/octicons/alert.svg";
            runsResultsList +=`<div class="mx-4 py-2 my-2 ${bb}"><div class="mb-3">
                    <a class="f6 text-bold link-gray-dark d-flex no-underline wb-break-all">${r.name}</a>
                    <p class="f6 text-gray mb-2"><img src="${statusIconPath}" class="ghpr-tabicon" alt=""/>
                        <b style="padding-left: 10px;">Status:</b> ${status}
                    </p>
                </div></div>`;
        }
        document.getElementById("all-runs").innerHTML = fullList;
        document.getElementById("recent-runs").innerHTML = recentList;
        document.getElementById("runs-results").innerHTML = runsResultsList;
    }

    private static updateRunsInfo(runs: Array<RunDto>, totalFiles: number): void {
        document.getElementById("total").innerHTML = `<b>Loaded runs:</b> ${runs.length}`;
        document.getElementById("loaded").innerHTML = `<b>Total runs:</b> ${totalFiles}`;
    }
    
    static updatePage(): void {
        Controller.init(PageType.TestRunsPage, (dataService: IDataService, reportSettings: ReportSettingsDto) => {
            dataService.fromPage(PageType.TestRunsPage).getLatestRuns((runs: Array<RunDto>, total: number) => {
                const latestRun = runs[0];
                DocumentHelper.setInnerHtmlById("project-name", reportSettings.projectName, "Awesome project");
                DocumentHelper.updateReportName(reportSettings.reportName);
                this.updateLatestRunInfo(latestRun);
                ReportPagePlotly.updatePlotlyBars(runs, "runs-bars");
                this.updateRunsInfo(runs, total);
                this.updateRunsList(runs);
                DocumentHelper.updateCopyright(reportSettings.coreVersion);
                window.addEventListener("resize", () => {
                    ReportPagePlotly.relayout("runs-bars");
                });
            });
        });
    }
    
    static initializePage(): void {
        this.updatePage();
        this.showTab("runs-stats", document.getElementById("tab-runs-stats"));
    }

    private static reportPageTabsIds: Array<string> = ["runs-stats", "runs-list"];

    static showTab(idToShow: string, caller: HTMLElement): void {
        TabsHelper.showTab(idToShow, caller, this.reportPageTabsIds);
    }
}