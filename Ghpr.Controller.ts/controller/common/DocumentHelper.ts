class DocumentHelper {

    public static updateCopyright(coreVersion: string): void {
        document.getElementById("copyright").innerHTML = `Copyright 2015 - 2019 © GhpReporter (version ${coreVersion})`;
    }

    public static setInnerHtmlById(id: string, value: string, defaultValue: string = ""): void {
        if (value === undefined) {
            value = defaultValue;
        }
        document.getElementById(id).innerHTML = value;
    }
}