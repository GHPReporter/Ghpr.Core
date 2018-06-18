using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class ItemInfoDtoMapper
    {
        public static ItemInfo MapTestRunInfo(this ItemInfoDto itemInfoDto, string itemName)
        {
            var testRun = new ItemInfo
            {
                ItemName = itemName,
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish
            };
            return testRun;
        }

        public static ItemInfo MapRunInfo(this ItemInfoDto itemInfoDto, string itemName)
        {
            var run = new ItemInfo
            {
                ItemName = itemName,
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish
            };
            return run;
        }
    }
}