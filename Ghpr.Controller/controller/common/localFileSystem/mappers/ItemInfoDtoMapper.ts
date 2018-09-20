///<reference path="./../entities/ItemInfo.ts"/>
///<reference path="./../../../dto/ItemInfoDto.ts"/>

class ItemInfoDtoMapper {
    static map(itemInfo: ItemInfo): ItemInfoDto {
        let itemIntoDto = new ItemInfoDto();
        itemIntoDto.guid = itemInfo.guid;
        itemIntoDto.start = itemInfo.start;
        itemIntoDto.finish = itemInfo.finish;
        itemIntoDto.itemName = itemInfo.itemName;
        return itemIntoDto;
    }
}