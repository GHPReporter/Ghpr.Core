///<reference path="./localFileSystem/entities/ItemInfo.ts"/>

class Sorter {
    static itemInfoSortByFinishDateFunc(a: ItemInfo, b: ItemInfo): number {
        if (a.finish > b.finish) {
            return 1;
        }
        if (a.finish < b.finish) {
            return -1;
        }
        return 0;
    }

    static itemInfoSortByFinishDateFuncDesc(a: ItemInfo, b: ItemInfo): number {
        if (a.finish < b.finish) {
            return 1;
        }
        if (a.finish > b.finish) {
            return -1;
        }
        return 0;
    }
}