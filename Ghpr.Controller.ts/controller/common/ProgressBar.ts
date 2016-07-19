///<reference path="./TabsHelper.ts"/>

class ProgressBar {

    barId = "progress-bar";
    barDivId = "progress-bar-div";
    barTextId = "progress-bar-line";

    private total: number;
    private current: number;

    constructor(total: number) {
        this.total = total;
        this.current = 0;
    }

    show(): void {
        console.log(`showing...`);
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
        console.log(`done.`);
    }

    onLoaded(count: number): void {
        this.current += count;
        const percentage = 100 * this.current / this.total;
        const pString = percentage.toString() + "%";
        document.getElementById(this.barDivId).style.width = pString;
        document.getElementById(this.barTextId).innerHTML = pString;
    }

    hide(): void {
        console.log(`hiding...`);
        document.getElementById(this.barId).innerHTML = "";
        document.getElementById(this.barId).style.display = "none";
        console.log(`done.`);
    }

}