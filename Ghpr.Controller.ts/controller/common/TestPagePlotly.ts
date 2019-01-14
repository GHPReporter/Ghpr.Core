///<reference path="./TestRunHelper.ts"/>
///<reference path="./PlotlyJs.ts"/>

class TestPagePlotly {
    static setTestHistory(tests: Array<TestRunDto>, currentTestNumber: number, id: string): void {
        const historyDiv = document.getElementById(id);
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
        const index = currentTestNumber;
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
        Plotly.newPlot(historyDiv, [historyTrace, currentTest], layout);
        (historyDiv as any).on("plotly_click", (eventData: any) => {
            var url = `${eventData.points[0].customdata}`;
            window.open(url, "_self");
        });
    }

    static getTestHistoryPlotSize(plotDiv: HTMLElement): any {
        const p = plotDiv.parentElement.parentElement.parentElement;
        const w = Math.max(300, Math.min(p.offsetWidth, 1100));
        const h = Math.max(300, Math.min(p.offsetHeight, 500));
        return { width: 0.95 * w, height: 0.95 * h };
    }

    static relayoutTestHistory(id: string): void {
        const historyDiv = document.getElementById(id);
        var size = this.getTestHistoryPlotSize(historyDiv);
        Plotly.relayout(historyDiv, { width: size.width, height: size.height });
    }
}