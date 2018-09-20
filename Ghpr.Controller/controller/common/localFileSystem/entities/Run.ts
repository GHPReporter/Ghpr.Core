///<reference path="./RunSummary.ts"/>
///<reference path="./ItemInfo.ts"/>

class Run {
    testRuns: Array<ItemInfo>;
    runInfo: ItemInfo;
    summary: RunSummary;
    name: string;
    sprint: string;
}