///<reference path="./UrlHelper.ts"/>

class TabsHelper {
    static showTab(idToShow: string, caller: HTMLElement, pageTabsIds: Array<string>): void {
        if (pageTabsIds.indexOf(idToShow) <= -1) {
            return;
        }
        UrlHelper.insertParam("currentTab", idToShow);
        const tabs = document.getElementsByClassName("ghpr-header-a");
        for (let i = 0; i < tabs.length; i++) {
            tabs[i].classList.remove("selected");
        }
        caller.className += " selected";
        pageTabsIds.forEach((id) => {
            document.getElementById(id).style.display = "none";
        });
        document.getElementById(idToShow).style.display = "";
    }
}