///<reference path="./../dto/TestRunDto.ts"/>
///<reference path="./../enums/TestResult.ts"/>
///<reference path="./Color.ts"/>

class TestRunHelper {
    static getColorByResult(result: TestResult): string {
        switch (result) {
            case TestResult.Passed:
                return Color.passed;
            case TestResult.Failed:
                return Color.failed;
            case TestResult.Broken:
                return Color.broken;
            case TestResult.Ignored:
                return Color.ignored;
            case TestResult.Inconclusive:
                return Color.inconclusive;
            case TestResult.Unknown:
                return Color.unknown;
            default:
                return "white";
        }
    }

    static getColor(t: TestRunDto): string {
        const result = this.getResult(t);
        return this.getColorByResult(result);
    }

    static getResult(t: TestRunDto): TestResult {
        if (t.result.indexOf("Passed") > -1) {
            return TestResult.Passed;
        }
        if (t.result.indexOf("Error") > -1) {
            return TestResult.Broken;
        }
        if (t.result.indexOf("Failed") > -1 || t.result.indexOf("Failure") > -1) {
            return TestResult.Failed;
        }
        if (t.result.indexOf("Inconclusive") > -1) {
            return TestResult.Inconclusive;
        }
        if (t.result.indexOf("Ignored") > -1 || t.result.indexOf("Skipped") > -1) {
            return TestResult.Ignored;
        }
        return TestResult.Unknown;
    }

    static getColoredResult(t: TestRunDto): string {
        return `<span class="p-1" style= "background-color: ${this.getColor(t)};" > ${t.result} </span>`;
    }

    static getColoredIns(v: string): string {
        return `<ins class="p-0" style= "background-color: ${Color.passed};text-decoration: none;" >${v}</ins>`;
    }

    static getColoredDel(v: string): string {
        return `<del class="p-0" style= "background-color: ${Color.failed};text-decoration: none;" >${v}</del>`;
    }

    static getOutput(t: TestRunDto): string {
        return t.output === "" ? "-" : t.output;
    }

    static getMessage(t: TestRunDto): string {
        return t.testMessage === "" ? "-" : t.testMessage;
    }

    static getDescription(t: TestRunDto): string {
        return (t.description === "" || t.description === undefined) ? "-" : t.description;
    }

    static getStackTrace(t: TestRunDto): string {
        return t.testStackTrace === "" ? "-" : t.testStackTrace;
    }

    static getCategories(t: TestRunDto): string {
        if (t.categories === undefined) {
            return "-";
        }
        return t.categories.length <= 0 ? "-" : t.categories.join(", ");
    }
}