using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class ItemInfoDtoMapper
    {
        public static ItemInfo MapTestRunInfo(this ItemInfoDto itemInfoDto)
        {
            var run = new ItemInfo
            {
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish
            };
            return run;
        }

        public static ItemInfo MapRunInfo(this ItemInfoDto itemInfoDto)
        {
            var run = new ItemInfo
            {
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish
            };
            return run;
        }
    }
}