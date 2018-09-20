class UrlHelper {
    static insertParam(key: string, value: string): void {
        const paramsPart = document.location.search.substr(1);
        window.history.pushState("", "", "");
        const p = `${key}=${value}`;
        if (paramsPart === "") {
            window.history.pushState("", "", `?${p}`);
        }
        else {
            let params = paramsPart.split("&");
            const paramToChange = params.find((par) => par.split("=")[0] === key);
            if (paramToChange != undefined) {
                if (params.length === 1) {
                    params = [p];
                }
                else {
                    const index = params.indexOf(paramToChange);
                    params.splice(index, 1);
                    params.push(p);
                }
            } else {
                params.push(p);
            }
            window.history.pushState("", "", `?${params.join("&")}`);
        }
    }

    static getParam(key: string): string {
        const paramsPart = document.location.search.substr(1);
        if (paramsPart === "") {
            return "";
        }
        else {
            const params = paramsPart.split("&");
            const paramToGet = params.find((par) => par.split("=")[0] === key);
            if (paramToGet != undefined) {
                return paramToGet.split("=")[1];
            } else {
                return "";
            }
        }
    }

    static removeParam(key: string): void {
        const paramsPart = document.location.search.substr(1);
        window.history.pushState("", "", "");
        if (paramsPart === "") {
            return;
        }
        else {
            let params = paramsPart.split("&");
            const paramToRemove = params.find((par) => par.split("=")[0] === key);
            if (paramToRemove != undefined) {
                const index = params.indexOf(paramToRemove);
                params.splice(index, 1);
            }
            window.history.pushState("", "", `?${params.join("&")}`);
        }
    }
}