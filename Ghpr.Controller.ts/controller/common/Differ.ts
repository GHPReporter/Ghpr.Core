///<reference path="./../interfaces/TestRun.ts"/>
///<reference path="./../enums/TestResult.ts"/>
///<reference path="./Color.ts"/>
///<reference path="./TestRunHelper.ts"/>

class Differ {
    static splitInclusive(str: string, sep: string, trim: boolean): string[] {
        if (!str.length) { return []; }
        let split = str.split(sep);
        if (trim) {
            split = split.filter((v: any) => v.length);
        }
        return split.map((line: string, idx: number, arr: string[]) => (idx < arr.length - 1 ? line + sep : line));
    }

    static splitInclusiveFull(str: string, sep: string, trim: boolean): string[] {
        if (!str.length) { return []; }
        let split = str.split(sep);
        if (trim) {
            split = split.filter((v: any) => v.length);
        }
        let res = new Array<string>();
        split.forEach((s: string, i: number, a: string[]) => {
            if (i < a.length - 1) {
                res.push(s);
                res.push(sep);
            } else {
                res.push(s);
            }
        });
        return res;
    }

    static splitArrInclusive(strs: string[], sep: string, trim: boolean): string[] {
        if (!strs.length) { return []; }
        let res = new Array<string>();
        strs.forEach((str: string, index: number, arr: string[]) => {
            const splittedStr = this.splitInclusiveFull(str, sep, trim);
            splittedStr.forEach((s: string, i: number, a: string[]) => {
                res.push(s);
            });
        });
        return res;
    }

    static splitInclusiveSeveral(str: string[], seps: string[], trim: boolean): any {
        if (!str.length) { return []; }
        let res: Array<string> = str;
        seps.forEach((sep: string, index: number, arr: string[]) => {
            const splittedStr = this.splitArrInclusive(res, sep, trim);
            res = splittedStr;
        });
        return res;
    }
    
    static  flattenRepeats(acc: any, cur: any) : any {
        if (acc.length && acc[acc.length - 1].value === cur) {
            acc[acc.length - 1].count++;
            return acc;
        }
        return acc.concat({
            value: cur,
            ref: -1,
            count: 1
        });
    }

    static addToTable(table: any, arr: any, type: any) : any {
        arr.forEach((token: any, idx: any) => {
            var ref = table;
            ref = ref[token.value] || (
                ref[token.value] = {}
            );
            ref = ref[token.count] || (
                ref[token.count] = {
                    left: -1,
                    right: -1
                }
            );
            if (ref[type] === -1) {
                ref[type] = idx;
            } else if (ref[type] >= 0) {
                ref[type] = -2;
            }
        });
    }

    static findUnique(table: any, left: any, right: any) : any {
        left.forEach((token: any) => {
            var ref = table[token.value][token.count];
            if (ref.left >= 0 && ref.right >= 0) {
                left[ref.left].ref = ref.right;
                right[ref.right].ref = ref.left;
            }
        });
    }

    static expandUnique(table: any, left: any, right: any, dir: any) : any {
        left.forEach((token: any, idx: any) => {
            if (token.ref === -1) { return; }
            var i = idx + dir, j = token.ref + dir,
                lx = left.length, rx = right.length;
            while (i >= 0 && j >= 0 && i < lx && j < rx) {
                // not checking counts here has a few subtle effects
                // this means that lines "next to" not-quite-exact (but repeated) lines
                // will be taken to be part of the span:
                // in [a f f c a, a f c a], the first 'a' will be marked as a pair
                // with the second one, because the 'f f' will be marked as a pair with 'f'
                // this is cleaned up when outputting the diff data: ['f f', 'f']
                // will become 'f -f' on output
                if (left[i].value !== right[j].value) {
                    break;
                }
                left[i].ref = j;
                right[j].ref = i;

                i += dir;
                j += dir;
            }
        });
    }

    static push(acc: any, token: any, type: any) : any {
        let n = token.count;
        while (n--) {
            acc.push({ type: type, value: token.value });
        }
    }

    static calcDist(lTarget: any, lPos: any, rTarget: any, rPos: any) : any {
        return (lTarget - lPos) + (rTarget - rPos) + Math.abs((lTarget - lPos) - (rTarget - rPos));
    }

    static processDiff(left: any, right: any) {
        var acc: any[] = [], lPos = 0, rPos = 0,
            lx = left.length, rx = right.length,
            lToken: any, rToken: any, lTarget: any, rTarget: any,
            rSeek: any, dist1: any, dist2: any;

        var countDiff: any;
        while (lPos < lx) {
            lTarget = lPos;
            // find the first sync-point on the left
            while (left[lTarget].ref < 0) {
                lTarget++;
            }
            rTarget = left[lTarget].ref;
            // left side referenced something we've already emitted, emit up to here
            if (rTarget < rPos) {
                // left-side un-referenced items are still deletions
                while (lPos < lTarget) {
                    this.push(acc, left[lPos++], "del");
                }
                // ... but since we've already emitted this change, this reference is void
                // and this token should be emitted as a deletion, not "same"
                this.push(acc, left[lPos++], "del");
                continue;
            }
            rToken = right[rTarget];
            dist1 = this.calcDist(lTarget, lPos, rTarget, rPos);
            for (rSeek = rTarget - 1; dist1 > 0 && rSeek >= rPos; rSeek--) {
                // if this isn't a paired token, keep seeking
                if (right[rSeek].ref < 0) { continue; }
                // if we've already emitted the referenced left-side token, keep seeking
                if (right[rSeek].ref < lPos) { continue; }
                // is this pair "closer" than the current pair?
                dist2 = this.calcDist(right[rSeek].ref, lPos, rSeek, rPos);
                if (dist2 < dist1) {
                    dist1 = dist2;
                    rTarget = rSeek;
                    lTarget = right[rSeek].ref;
                }
            }

            // emit deletions
            while (lPos < lTarget) {
                this.push(acc, left[lPos++], "del");
            }

            // emit insertions
            while (rPos < rTarget) {
                this.push(acc, right[rPos++], "ins");
            }

            // we're done when we hit the pseudo-token on the left
            if ("eof" in left[lPos]) { break; }

            // emit synced pair
            // since we allow repeats of different lengths to be matched
            // via the pass 4 & 5 expansion, we need to ensure we emit
            // the correct sequence when the counts don't align
            countDiff = left[lPos].count - right[rPos].count;
            if (countDiff === 0) {
                this.push(acc, left[lPos], "same");
            } else if (countDiff < 0) {
                // more on the right than the left: some same, some insertion
                this.push(acc, {
                    count: right[rPos].count + countDiff,
                    value: right[rPos].value
                }, "same");
                this.push(acc, {
                    count: -countDiff,
                    value: right[rPos].value
                }, "ins");
            } else if (countDiff > 0) {
                // more on the left than the right: some same, some deletion
                this.push(acc, {
                    count: left[lPos].count - countDiff,
                    value: left[lPos].value
                }, "same");
                this.push(acc, {
                    count: countDiff,
                    value: left[lPos].value
                }, "del");
            }
            lPos++;
            rPos++;
        }
        return acc;
    }

    static  same(left: any, right: any) {
        if (left.length !== right.length) { return false; }
        return left.reduce((acc: any, cur: any, idx: any) => (acc && cur === right[idx]), true);
    };

    static all(type: any) {
        return (val: any) => ({
            type: type,
            value: val
        });
    }

    static  diff(leftLines: any, rightLines: any) {
        let left = (leftLines && Array.isArray(leftLines) ? leftLines : []);
        let right = (rightLines && Array.isArray(rightLines) ? rightLines : []);
        // if they're the same, no need to do all that work
        if (this.same(leftLines, rightLines)) {
            return left.map(this.all("same"));
        }
        if (left.length === 0) {
            return right.map(this.all("ins"));
        }
        if (right.length === 0) {
            return left.map(this.all("del"));
        }
        left = left.reduce(this.flattenRepeats, []);
        right = right.reduce(this.flattenRepeats, []);
        let table = {};
        this.addToTable(table, left, "left");
        this.addToTable(table, right, "right");
        this.findUnique(table, left, right);
        this.expandUnique(table, left, right, 1);
        this.expandUnique(table, left, right, -1);
        left.push({ ref: right.length, eof: true }); // include trailing deletions
        table = null;
        const res = this.processDiff(left, right);
        left = null;
        right = null;
        return res;
    }

    static  accumulateChanges(changes: any, fn: any) {
        var del: any[] = [], ins: any[] = [];
        changes.forEach((change: any) => {
            if (change.type === "del") { del.push(change.value); }
            if (change.type === "ins") { ins.push(change.value); }
        });
        if (!del.length || !ins.length) { return changes; }
        return fn(del.join(""), ins.join(""));
    }

    static refineChanged(changes: any, fn: any) {
        var ptr = -1;

        return changes.concat({
            type: "same",
            eof: true
        }).reduce((acc: any, cur: any, idx : number, a: any) => {
            var part: any[] = [];

            if (cur.type === "same") {
                if (ptr >= 0) {
                    part = this.accumulateChanges(a.slice(ptr, idx), fn);
                    if (a[idx - 1].type !== "ins") {
                        part = a.slice(ptr, idx);
                    } else {
                    }
                    ptr = -1;
                }
                return acc.concat(part).concat(cur.eof ? [] : [cur]);
            } else if (ptr < 0) {
                ptr = idx;
            }
            return acc;
        }, []);
    }
    
    static minimize(changes: any) : any {
        var del: any[] = [], ins: any[] = [];
        return changes.concat({ type: "same", eof: true })
            .reduce((acc: any, cur: any) => {
                if (cur.type === "del") {
                    del.push(cur.value);
                    return acc;
                }
                if (cur.type === "ins") {
                    ins.push(cur.value);
                    return acc;
                }
                if (del.length) {
                    acc.push({
                        type: "del",
                        value: del.join("")
                    });
                    del = [];
                }
                if (ins.length) {
                    acc.push({
                        type: "ins",
                        value: ins.join("")
                    });
                    ins = [];
                }
                if (cur.eof !== true) {
                    if (acc.length && acc[acc.length - 1].type === "same") {
                        acc[acc.length - 1].value += cur.value;
                    } else {
                        acc.push(cur);
                    }
                }
                return acc;
            }, []);
    }

    static diffLines(left: any, right: any, trim: any) {
        return this.diff(
            this.splitInclusiveFull(left, "\n", trim),
            this.splitInclusiveFull(right, "\n", trim)
        );
    }

    static diffWords(left: any, right: any, trim: any) {
        return this.diff(
            this.splitInclusiveFull(left, " ", trim),
            this.splitInclusiveFull(right, " ", trim)
        );
    }

    static separators: string[] = [" ", "<", ">", "/", ".", "?", "!"];

    static diffLetters(left: any, right: any, trim: any) {
        return this.diff(
            this.splitInclusiveSeveral([left], this.separators, trim),
            this.splitInclusiveSeveral([right], this.separators, trim)
        );
    }

    static diffChars(left: any, right: any, trim: any) {
        return this.diff(
            this.splitInclusiveFull(left, "", trim),
            this.splitInclusiveFull(right, "", trim)
        );
    }

    static diffHybrid(left: any, right: any, trim: any) {
        return this.refineChanged(
            this.diffLines(left, right, trim),
            (del: any, ins: any) => this.diffLetters(del, ins, trim)
        );
    }

    static replaceTag(tag: string) : string {
        const tagsToReplace : any = {
            '&': "&amp;",
            '<': "&lt;",
            '>': "&gt;",
            '"': "&quot;",
            "'": "&#39;",
            '/': "&#x2F;",
            '`': "&#x60;",
            '=': "&#x3D;"
        };
        return tagsToReplace[tag] || tag;
    }

    static safeTagsReplace(str: string) : string{
        return str.replace(/[&<>]/g, this.replaceTag);
    }

    static getHtmlForOneChange(change: any): string {
        let res = "";
        const v = this.safeTagsReplace(change.value);
        if (change.value === "") {
            res = "";
        }
        if (change.type === "same") {
            res = `${v}`;
        }
        if (change.type === "ins") {
            res = TestRunHelper.getColoredIns(v);
        }
        if (change.type === "del") {
            res = TestRunHelper.getColoredDel(v);
        }
        return res;
    }

    static getHtml(left: string, right: string): string {
        let res = "";
        const changes = this.diffHybrid(left, right, false);
        changes.forEach((change: any) => {
             res += this.getHtmlForOneChange(change);
        });
        res = `<div style="word-wrap: break-word;  white-space: pre-wrap;">${res}</div>`; 
        return res;
    }
}
