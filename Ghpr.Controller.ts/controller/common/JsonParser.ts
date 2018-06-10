class JsonParser {
    static reviveRun(key: any, value: any): any {
        if (key === "start" || key === "finish" || key === "date") return new Date(value);
        //if (key === "duration") return new Number(value);
        return value;
    }
}