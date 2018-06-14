///<reference path="./RunSummaryDto.ts"/>
///<reference path="./ItemInfoDto.ts"/>

class RunDto {
    testsInfo: Array<ItemInfoDto>;
    runInfo: ItemInfoDto;
    summary: RunSummaryDto;
    name: string;
    sprint: string;
}