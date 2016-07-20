///<reference path="./../interfaces/ITestRun.ts"/>
///<reference path="./../enums/TestResult.ts"/>
///<reference path="./Color.ts"/>

class TestRunHelper {
    static getColor(t: ITestRun): string {
        const result = this.getResult(t);
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
            default:
                return Color.unknown;
        }
    }

    static getResult(t: ITestRun): TestResult {
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

    static getColoredResult(t: ITestRun): string {
        return `<span class="p-1" style= "background-color: ${this.getColor(t)};" > ${t.result} </span>`;
    }

    static getOutput(t: ITestRun): string {
        return t.output === "" ? "-" : t.output;
    }

    static getMessage(t: ITestRun): string {
        return t.testMessage === "" ? "-" : t.testMessage;
    }

    static getStackTrace(t: ITestRun): string {
        return t.testStackTrace === "" ? "-" : t.testStackTrace;
    }

    static getCategories(t: ITestRun): string {
        if (t.categories === undefined) {
            return "-";
        }
        return t.categories.length <= 0 ? "-" : t.categories.join(", ");
    }
}