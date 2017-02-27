class DateFormatter {
    static format(date: Date): string {
        if (date < new Date(2000, 1)) {
            return "-";
        }
        const year = `${date.getFullYear()}`;
        const month = this.correctString(`${date.getMonth() + 1}`);
        const day = this.correctString(`${date.getDate()}`);
        const hour = this.correctString(`${date.getHours()}`);
        const minute = this.correctString(`${date.getMinutes()}`);
        const second = this.correctString(`${date.getSeconds()}`);
        return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
    }

    static diff(start: Date, finish: Date): string {
        const timeDifference = (finish.getTime() - start.getTime());
        const dDate = new Date(timeDifference);
        const dHours = dDate.getUTCHours();
        const dMins = dDate.getUTCMinutes();
        const dSecs = dDate.getUTCSeconds();
        const dMilliSecs = dDate.getUTCMilliseconds();
        const readableDifference = this.correctNumber(dHours) + ":" + this.correctNumber(dMins) + ":"
            + this.correctNumber(dSecs) + "." + this.correctNumber(dMilliSecs);
        return readableDifference;
    }

    static correctString(s: string): string {
        if (s.length === 1) {
            return `0${s}`;
        } else return s;
    }

    static correctNumber(n: number): string {
        if (n >= 0 && n < 10) {
            return `0${n}`;
        } else return `${n}`;
    }
}