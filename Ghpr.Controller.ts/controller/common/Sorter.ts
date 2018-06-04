///<reference path="./../interfaces/ItemInfo.ts"/>

class Sorter {
    static itemInfoSorterByFinishDateFunc(a: ItemInfo, b: ItemInfo): number {
        if (a.finish > b.finish) {
            return 1;
        }
        if (a.finish < b.finish) {
            return -1;
        }
        return 0;
    }

    static itemInfoSorterByFinishDateFuncDesc(a: ItemInfo, b: ItemInfo): number {
        if (a.finish < b.finish) {
            return 1;
        }
        if (a.finish > b.finish) {
            return -1;
        }
        return 0;
    }
}