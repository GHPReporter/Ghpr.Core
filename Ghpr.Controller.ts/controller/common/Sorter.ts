///<reference path="./localFileSystem/entities/ItemInfo.ts"/>

class Sorter {
    static itemInfoByFinishDate(a: ItemInfo, b: ItemInfo): number {
        if (a.finish > b.finish) {
            return 1;
        }
        if (a.finish < b.finish) {
            return -1;
        }
        return 0;
    }

    static itemInfoByFinishDateDesc(a: ItemInfo, b: ItemInfo): number {
        if (a.finish < b.finish) {
            return 1;
        }
        if (a.finish > b.finish) {
            return -1;
        }
        return 0;
    }
}