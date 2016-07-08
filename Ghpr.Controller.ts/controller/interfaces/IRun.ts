///<reference path="./IRunSummary.ts"/>
///<reference path="./IItemInfo.ts"/>

interface IRun {
    testRunFiles: Array<string>;
    runInfo: IItemInfo;
    summary: IRunSummary;
    name: string;
    sprint: string;
}