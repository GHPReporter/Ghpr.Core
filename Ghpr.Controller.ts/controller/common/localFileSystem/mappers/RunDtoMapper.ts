///<reference path="./../entities/Run.ts"/>
///<reference path="./../../../dto/RunDto.ts"/>
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

        let testRuns = run.testRuns;
        let len = testRuns.length;
        let testInfoDtos = new Array<ItemInfoDto>(len);
        for (let i = 0; i < len; i++) {
            testInfoDtos[i] = ItemInfoDtoMapper.map(testRuns[i]);
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