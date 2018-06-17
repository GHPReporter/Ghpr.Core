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

    static formatWithMs(date: Date): string {
        if (date < new Date(2000, 1)) {
            return "-";
        }
        const ms = this.correctMs(date.getMilliseconds());
        return this.format(date) + "." + ms;
    }

    static toFileFormat(date: Date): string {
        if (date.getFullYear() === 1) {
            return "00010101_000000000";
        }
        const year = this.correctYear(date.getUTCFullYear());
        const month = this.correctString(`${date.getUTCMonth() + 1}`);
        const day = this.correctString(`${date.getUTCDate()}`);
        const hour = this.correctString(`${date.getUTCHours()}`);
        const minute = this.correctString(`${date.getUTCMinutes()}`);
        const second = this.correctString(`${date.getUTCSeconds()}`);
        const ms = this.correctMs(date.getUTCMilliseconds());
        let result = year + month + day + "_" + hour + minute + second + ms;
        return result;
    }

    static fromFileFormat(fileFormatDate: string): Date {
        //fileFormatDate: yyyyMMdd_hhmmssfff
        if (fileFormatDate === "00010101_000000000") {
            return new Date("0001-01-01");
        }
        let date = fileFormatDate.split("_")[0];
        let time = fileFormatDate.split("_")[1];
        let dateFromFile = new Date(Date.UTC(+date.substr(0, 4),
            +date.substr(4, 2) - 1,
            +date.substr(6, 2),
            +time.substr(0, 2),
            +time.substr(2, 2),
            +time.substr(4, 2),
            +time.substr(6, 3)));
        return dateFromFile;
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

    static correctMs(n: number): string {
        if (n >= 0 && n < 10) {
            return `00${n}`;
        } else if (n >= 10 && n < 100) {
            return `0${n}`;
        } else return `${n}`;
    }

    static correctYear(n: number): string {
        if (n >= 0 && n < 10) {
            return `000${n}`;
        } else if (n >= 10 && n < 100) {
            return `00${n}`;
        } else if (n >= 100 && n < 1000) {
            return `0${n}`;
        } else return `${n}`;
    }
}