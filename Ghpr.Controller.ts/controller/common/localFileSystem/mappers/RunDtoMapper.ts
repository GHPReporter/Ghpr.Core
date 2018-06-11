///<reference path="./../entities/Run.ts"/>
///<reference path="./../../../dto/RunDto.ts"/>
///<reference path="./../entities/RunSummary.ts"/>
///<reference path="./../../../dto/RunSummaryDto.ts"/>
///<reference path="./ItemInfoDtoMapper.ts"/>

class RunDtoMapper {
    static map(run: Run): RunDto {
        let runSummaryDto = new RunSummaryDto();
        runSummaryDto.errors = run.summary.errors;
        runSummaryDto.failures = run.summary.failures;
        runSummaryDto.ignored = run.summary.ignored;
        runSummaryDto.inconclusive = run.summary.inconclusive;
        runSummaryDto.success = run.summary.success;
        runSummaryDto.unknown = run.summary.unknown;
        runSummaryDto.total = run.summary.total;

        let files = run.testRunFiles;
        let testsInfoDto = new Array<ItemInfoDto>();
        for (let testRunFile in files) {
            let testInfoDto = new ItemInfoDto();
            testInfoDto.guid = testRunFile.split("\\")[0];
            var date = testRunFile.split("\\")[1].split(".")[0].split("_")[1];
            var time = testRunFile.split("\\")[1].split(".")[0].split("_")[2];
            testInfoDto.finish = new Date(+date.substr(0, 4), 10, 20, 19, 45, 55);
        }

        let runDto = new RunDto();
        runDto.name = run.name;
        runDto.runInfo = ItemInfoDtoMapper.map(run.runInfo);
        runDto.sprint = run.sprint;
        runDto.summary = runSummaryDto;
        runDto.testsInfo = testsInfoDto;
        return runDto;
    }
}