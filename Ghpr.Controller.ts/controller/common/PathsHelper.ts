///<reference path="./../enums/PageType.ts"/>

class PathsHelper {
    static getRunPath(pt: PageType, guid: string): string {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./runs/run_${guid}.json`;
            case PageType.TestRunPage:
                return `./run_${guid}.json`;
            case PageType.TestPage:
                return `./../../runs/run_${guid}.json`;
            default:
                return "";
        }
    }

    static getRunsPath(pt: PageType): string {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./runs/runs.json`;
            case PageType.TestRunPage:
                return `./runs.json`;
            case PageType.TestPage:
                return `./../../runs/runs.json`;
            default:
                return "";
        }
    }

    static getReportSettingsPath(pt: PageType): string {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./src/ReportSettings.json`;
            case PageType.TestRunPage:
                return `./../src/ReportSettings.json`;
            case PageType.TestPage:
                return `./../src/ReportSettings.json`;
            default:
                return "";
        }
    }

    static getTestsPath(testGuid: string, pt: PageType): string {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./tests/${testGuid}/tests.json`;
            case PageType.TestRunPage:
                return `./../tests/${testGuid}/tests.json`;
            case PageType.TestPage:
                return `./${testGuid}/tests.json`;
            default:
                return "";
        }
    }

    static getTestPath(testGuid: string, testFileName: string, pt: PageType): string {
        switch (pt) {
            case PageType.TestRunsPage:
                return `./tests/${testGuid}/${testFileName}`;
            case PageType.TestRunPage:
                return `./../tests/${testGuid}/${testFileName}`;
            case PageType.TestPage:
                return `./${testGuid}/${testFileName}`;
            default:
                return "";
        }
    }
}