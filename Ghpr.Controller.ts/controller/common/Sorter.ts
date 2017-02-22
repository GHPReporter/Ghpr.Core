///<reference path="./../interfaces/IItemInfo.ts"/>

class Sorter {
    static itemInfoSorterByFinishDateFunc(a: IItemInfo, b: IItemInfo): number {
        if (a.finish > b.finish) {
            return 1;
        }
        if (a.finish < b.finish) {
            return -1;
        }
        return 0;
    }

    static itemInfoSorterByFinishDateFuncDesc(a: IItemInfo, b: IItemInfo): number {
        if (a.finish < b.finish) {
            return 1;
        }
        if (a.finish > b.finish) {
            return -1;
        }
        return 0;
    }
}