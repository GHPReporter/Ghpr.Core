using Ghpr.Core.Core.Common;
using Ghpr.LocalFileSystem.Core.Entities;
using Ghpr.LocalFileSystem.Core.Providers;

namespace Ghpr.LocalFileSystem.Core.Mappers
{
    public static class ItemInfoDtoMapper
    {
        public static ItemInfo MapTestRunInfo(this ItemInfoDto itemInfoDto)
        {
            var run = new ItemInfo
            {
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish,
                ItemName = NamesProvider.GetTestRunFileName(itemInfoDto.Finish)
            };
            return run;
        }

        public static ItemInfo MapRunInfo(this ItemInfoDto itemInfoDto)
        {
            var run = new ItemInfo
            {
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish,
                ItemName = NamesProvider.GetRunFileName(itemInfoDto.Guid)
            };
            return run;
        }

        public static ItemInfoDto ToDto(this ItemInfo itemInfoDto)
        {
            var res = new ItemInfoDto
            {
                Guid = itemInfoDto.Guid,
                Start = itemInfoDto.Start,
                Finish = itemInfoDto.Finish,
                ItemName = itemInfoDto.ItemName
            };
            return res;
        }
    }
}