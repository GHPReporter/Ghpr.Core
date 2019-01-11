class DocumentHelper {

    public static updateCopyright(coreVersion: string): void {
        this.setInnerHtmlById("copyright", `Copyright 2015 - 2019 © GhpReporter (version ${coreVersion})`);
    }

    public static updateReportName(reportName: string): void {
        this.setInnerHtmlById("report-name", reportName, "GHPReport");
    }

    public static setInnerHtmlById(id: string, value: string, defaultValue = ""): void {
        if (value === undefined) {
            value = defaultValue;
        }
        document.getElementById(id).innerHTML = value;
    }
}