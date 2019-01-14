///<reference path="./../dto/RunDto.ts"/>
///<reference path="./Color.ts"/>
///<reference path="./PlotlyJs.ts"/>

class RunPagePlotly {

    static getSummaryPlotSize(plotDiv: HTMLElement): any {
        var p = plotDiv.parentElement;
        var w = Math.max(300, Math.min(p.offsetWidth, 800));
        var h = Math.max(400, Math.min(p.offsetHeight, 500));
        return { width: 0.95 * w, height: 0.95 * h };
    }

    static getTimelinePlotSize(plotDiv: HTMLElement): any {
        var p = plotDiv.parentElement.parentElement.parentElement;
        var w = Math.max(300, Math.min(p.offsetWidth, 1000));
        var h = Math.max(400, Math.min(p.offsetHeight, 500));
        return { width: 1.00 * w, height: 1.00 * h };
    }

    static updateTimeline(data: any[], id: string): void {
        const timelineDiv = document.getElementById(id);
        var size = this.getTimelinePlotSize(timelineDiv);
        var layout = {
            title: "Timeline",
            yaxis: {
                showgrid: false,
                zeroline: false,
                showline: false,
                showticklabels: false
            },
            width: size.width,
            height: size.height
        };
        Plotly.react(timelineDiv, data, layout);
    }

    static updateSummary(run: RunDto, id: string): void {
        const s = run.summary;
        const pieDiv = document.getElementById(id);
        var size = this.getSummaryPlotSize(pieDiv);
        var data = [
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
        ];
        var layout = {
            margin: { t: 20 },
            width: size.width,
            height: size.height
        };
        Plotly.react(pieDiv, data, layout);
    }

    static relayoutSummaryPlot(id: string): void {
        const summaryPieDiv = document.getElementById(id);
        var summarySize = this.getSummaryPlotSize(summaryPieDiv);
        Plotly.relayout(summaryPieDiv, { width: summarySize.width, height: summarySize.height });
    }

    static relayoutTimelinePlot(id: string): void {
        const timelinePieDiv = document.getElementById(id);
        var timelineSize = this.getTimelinePlotSize(timelinePieDiv);
        Plotly.relayout(timelinePieDiv, { width: timelineSize.width, height: timelineSize.height });
    }
}