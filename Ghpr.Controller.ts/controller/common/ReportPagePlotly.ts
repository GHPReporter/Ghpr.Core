///<reference path="./PlotlyJs.ts"/>
///<reference path="./../dto/RunDto.ts"/>
///<reference path="./Color.ts"/>

class ReportPagePlotly {

    static updatePlotlyBars(runs: Array<RunDto>, id: string): void {
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

        const runGuids: Array<string> = new Array();

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

            let ri = runs[i].runInfo;
            runGuids[i] = `${ri.guid}`;
        }
        const t = "bar";
        const hi = "y";
        plotlyData = [
            { x: unknownX, y: unknownY, name: "unknown", customdata: runGuids, type: t, hoverinfo: hi, marker: { color: Color.unknown } },
            { x: inconclX, y: inconclY, name: "inconclusive", customdata: runGuids, type: t, hoverinfo: hi, marker: { color: Color.inconclusive } },
            { x: ignoredX, y: ignoredY, name: "ignored", customdata: runGuids, type: t, hoverinfo: hi, marker: { color: Color.ignored } },
            { x: brokenX, y: brokenY, name: "broken", customdata: runGuids, type: t, hoverinfo: hi, marker: { color: Color.broken } },
            { x: failedX, y: failedY, name: "failed", customdata: runGuids, type: t, hoverinfo: hi, marker: { color: Color.failed } },
            { x: passedX, y: passedY, name: "passed", customdata: runGuids, type: t, hoverinfo: hi, marker: { color: Color.passed } }
        ];

        const barsDiv = document.getElementById(id);

        var size = this.getPlotSize(barsDiv);

        var layout = {
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
            bargap: 0.01,
            width: size.width,
            height: size.height
        };

        Plotly.react(barsDiv, plotlyData, layout);

        (barsDiv as any).on("plotly_click", (eventData: any) => {
            var url = `./runs/index.html?runGuid=${eventData.points[0].customdata}`;
            var win = window.open(url, "_blank");
            win.focus();
        });
    }
    
    static getPlotSize(plotDiv: HTMLElement): any {
        var p = plotDiv.parentElement.parentElement.parentElement.parentElement;
        var w = Math.max(300, Math.min(p.offsetWidth, 1000));
        var h = Math.max(400, Math.min(p.offsetHeight, 700));
        return { width: 0.95 * w, height: 0.85 * h };
    }

    static relayout(id: string): void {
        const barsDiv = document.getElementById(id);
        var size = this.getPlotSize(barsDiv);
        Plotly.relayout(barsDiv, { width: size.width, height: size.height });
    }
}