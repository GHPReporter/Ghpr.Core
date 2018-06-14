///<reference path="./RunSummary.ts"/>
///<reference path="./ItemInfo.ts"/>

class Run {
    testRunFiles: Array<string>;
    runInfo: ItemInfo;
    summary: RunSummary;
    name: string;
    sprint: string;
}