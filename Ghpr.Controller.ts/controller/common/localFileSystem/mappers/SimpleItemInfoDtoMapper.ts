///<reference path="./../entities/SimpleItemInfo.ts"/>
///<reference path="./../../../dto/SimpleItemInfoDto.ts"/>

class SimpleItemInfoDtoMapper {
    static map(simpleItemInfo: SimpleItemInfo): SimpleItemInfoDto {
        let simpleItemIntoDto = new SimpleItemInfoDto();
        simpleItemIntoDto.date = simpleItemInfo.date;
        simpleItemIntoDto.itemName = simpleItemInfo.itemName;
        return simpleItemIntoDto;
    }
}