///<reference path="./localFileSystem/entities/ItemInfo.ts"/>
///<reference path="./localFileSystem/entities/ReportSettings.ts"/>
///<reference path="./localFileSystem/entities/Run.ts"/>
///<reference path="./../enums/PageType.ts"/>
///<reference path="./localFileSystem/JsonLoader.ts"/>
///<reference path="./JsonParser.ts"/>
///<reference path="./UrlHelper.ts"/>
///<reference path="./DateFormatter.ts"/>
///<reference path="./Color.ts"/>
///<reference path="./PlotlyJs.ts"/>
///<reference path="./../Controller.ts"/>

class ReportPageUpdater {

    static reviveRun = JsonParser.reviveRun;

    private static updateLatestRunInfo(latestRun: RunDto): void {
        document.getElementById("start").innerHTML = `<b>Start datetime:</b> ${DateFormatter.format(latestRun.runInfo.start)}`;
        document.getElementById("finish").innerHTML = `<b>Finish datetime:</b> ${DateFormatter.format(latestRun.runInfo.finish)}`;
        document.getElementById("duration").innerHTML = `<b>Duration:</b> ${DateFormatter.diff(latestRun.runInfo.start, latestRun.runInfo.finish)}`;
    }

    private static updateCopyright(coreVersion: string): void {
        document.getElementById("copyright").innerHTML = `Copyright 2015 - 2018 © GhpReporter (version ${coreVersion})`;
    }

    private static updateRunsList(runs: Array<RunDto>): void {
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

    private static updateRunsInfo(runs: Array<RunDto>, totalFiles: number): void {
        document.getElementById("total").innerHTML = `<b>Loaded runs:</b> ${runs.length}`;
        document.getElementById("loaded").innerHTML = `<b>Total runs:</b> ${totalFiles}`;
    }
    
    private static updatePlotlyBars(runs: Array<RunDto>): void {
        let plotlyData = new Array();
        const passedY: Array<number> = new Array();
        const failedY: Array<number> = new Array();
        const brokenY: Array<number> = new Array();
        const inconclY: Array<number> = new Array();
        const ignoredY: Array<number> = new Array();
        const unknownY: Array<number> = new Array();

        const passedX: Array<number> = new Array();
        const failedX: Array<number> = new Array();
        const brokenX: Array<number> = new Array();
        const inconclX: Array<number> = new Array();
        const ignoredX: Array<number> = new Array();
        const unknownX: Array<number> = new Array();

        const tickvals: Array<number> = new Array();
        const ticktext: Array<string> = new Array();

        const c = runs.length;
        for (let i = 0; i < c; i++) {
            let s = runs[i].summary;
            //let ri = runs[i].runInfo;
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
            ticktext[i] = `run ${j}`;// `run ${j} ${ri.start}`;
        }
        const t = "bar";
        const hi = "y";
        plotlyData = [
            { x: unknownX, y: unknownY, name: "unknown",      type: t, hoverinfo: hi, marker: { color: Color.unknown } },
            { x: inconclX, y: inconclY, name: "inconclusive", type: t, hoverinfo: hi, marker: { color: Color.inconclusive } },
            { x: ignoredX, y: ignoredY, name: "ignored",      type: t, hoverinfo: hi, marker: { color: Color.ignored } },
            { x: brokenX,  y: brokenY,  name: "broken",       type: t, hoverinfo: hi, marker: { color: Color.broken } },
            { x: failedX,  y: failedY,  name: "failed",       type: t, hoverinfo: hi, marker: { color: Color.failed } },
            { x: passedX,  y: passedY,  name: "passed",       type: t, hoverinfo: hi, marker: { color: Color.passed } }
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

    static updatePage(): void {
        Controller.init(PageType.TestRunsPage, (dataService: IDataService, reportSettings: ReportSettingsDto) => {
            dataService.fromPage(PageType.TestRunsPage).getLatestRuns((runs: Array<RunDto>, total: number) => {
                const latestRun = runs[0];
                this.updateLatestRunInfo(latestRun);
                this.updatePlotlyBars(runs);
                this.updateRunsInfo(runs, total);
                this.updateRunsList(runs);
                this.updateCopyright(reportSettings.coreVersion);
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