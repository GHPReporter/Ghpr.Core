///<reference path="./RunSummaryDto.ts"/>
///<reference path="./ItemInfoDto.ts"/>

class RunDto {
    testRunFiles: Array<string>;
    runInfo: ItemInfoDto;
    summary: RunSummaryDto;
    name: string;
    sprint: string;
}