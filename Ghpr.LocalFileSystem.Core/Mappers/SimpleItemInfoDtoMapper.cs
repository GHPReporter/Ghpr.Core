using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class SimpleItemInfoDtoMapper
    {
        public static SimpleItemInfo MapSimpleItemInfo(this SimpleItemInfoDto simpleItemInfoDto, string itemName = "")
        {
            var simpleItemInfo = new SimpleItemInfo
            {
                ItemName = itemName.Equals("") ? simpleItemInfoDto.ItemName : itemName,
                Date = simpleItemInfoDto.Date
            };
            return simpleItemInfo;
        }

        public static SimpleItemInfoDto ToDto(this SimpleItemInfo simpleItemInfo)
        {
            var dto = new SimpleItemInfoDto
            {
                ItemName = simpleItemInfo.ItemName,
                Date = simpleItemInfo.Date
            };
            return dto;
        }
    }
}