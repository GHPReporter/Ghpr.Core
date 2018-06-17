///<reference path="./../entities/Run.ts"/>
///<reference path="./../../../dto/RunDto.ts"/>
///<reference path="./../entities/RunSummary.ts"/>
///<reference path="./../../../dto/RunSummaryDto.ts"/>
///<reference path="./ItemInfoDtoMapper.ts"/>
///<reference path="./../../DateFormatter.ts"/>

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
            let temp = testRunFile.split("\\")[1].split(".")[0].split("_");
            console.log("MAPPER: ");
            testInfoDto.finish = DateFormatter.fromFileFormat(temp[1] + "_" + temp[2]);
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