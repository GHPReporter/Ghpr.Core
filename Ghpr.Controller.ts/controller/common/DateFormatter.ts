class DateFormatter {
    static format(date: Date): string {
        if (date < new Date(2000, 1)) {
            return "-";
        }
        const year = `${date.getFullYear()}`;
        const month = DateFormatter.correctString(`${date.getMonth() + 1}`);
        const day = DateFormatter.correctString(`${date.getDate()}`);
        const hour = DateFormatter.correctString(`${date.getHours()}`);
        const minute = DateFormatter.correctString(`${date.getMinutes()}`);
        const second = DateFormatter.correctString(`${date.getSeconds()}`);
        return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
    }

    static diff(start: Date, finish: Date): string {
        const timeDifference = (finish.getTime() - start.getTime());
        const dDate = new Date(timeDifference);
        const dHours = dDate.getUTCHours();
        const dMins = dDate.getUTCMinutes();
        const dSecs = dDate.getUTCSeconds();
        const dMilliSecs = dDate.getUTCMilliseconds();
        const readableDifference = dHours + ":" + dMins + ":" + dSecs + "." + dMilliSecs;
        return readableDifference;
    }

    static correctString(s: string): string {
        if (s.length === 1) {
            return `0${s}`;
        } else return s;
    }
}