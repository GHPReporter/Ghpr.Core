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
        let testInfoDtos = new Array<ItemInfoDto>(files.length);
        for (let i = 0; i < files.length; i++) {
            let testRunFile = files[i];
            let testInfoDto = new ItemInfoDto();
            testInfoDto.guid = testRunFile.split("\\")[0];
            let date = testRunFile.split("\\")[1].split(".")[0].split("_")[1];
            let time = testRunFile.split("\\")[1].split(".")[0].split("_")[2];
            if (date.substr(0, 4) !== "0001") {
                testInfoDto.finish = new Date(+date.substr(0, 4),
                    +date.substr(4, 2) - 1,
                    +date.substr(6, 2),
                    +time.substr(0, 2),
                    +time.substr(2, 2),
                    +time.substr(4, 2),
                    +time.substr(6, 3));
            } else {
                testInfoDto.finish = new Date("0001-01-01");
            }
            testInfoDto.start = new Date();
            testInfoDtos[i] = testInfoDto;
        }

        let runDto = new RunDto();
        runDto.name = run.name;
        runDto.runInfo = ItemInfoDtoMapper.map(run.runInfo);
        runDto.sprint = run.sprint;
        runDto.summary = runSummaryDto;
        runDto.testsInfo = testInfoDtos;
        return runDto;
    }
}