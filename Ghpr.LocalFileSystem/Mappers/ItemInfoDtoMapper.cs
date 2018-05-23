using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Mappers
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
                FileName = LocationsProvider.GetTestRunFileName(itemInfoDto.Finish)
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
                FileName = LocationsProvider.GetRunFileName(itemInfoDto.Guid)
            };
            return run;
        }
    }
}