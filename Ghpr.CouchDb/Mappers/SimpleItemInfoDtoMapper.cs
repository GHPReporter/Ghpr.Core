using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
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

        public static SimpleItemInfoDto ToDto(this SimpleItemInfo entity)
        {
            var dto = new SimpleItemInfoDto
            {
                ItemName = entity.ItemName,
                Date = entity.Date
            };
            return dto;
        }
    }
}