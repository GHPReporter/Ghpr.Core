using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class TestScreenshotMapper
    {
        public static DatabaseEntity<TestScreenshot> Map(this TestScreenshotDto testScreenshotDto)
        {
            var id = $"screenshot-{testScreenshotDto.TestGuid.ToString()}" +
                     $"-{testScreenshotDto.TestScreenshotInfo.Date:yyyyMMdd_HHmmssfff}";
            var testScreenshot = new TestScreenshot
            {
                TestScreenshotInfo = testScreenshotDto.TestScreenshotInfo.MapSimpleItemInfo(id),
                Base64Data = testScreenshotDto.Base64Data,
                TestGuid = testScreenshotDto.TestGuid,
                Format = testScreenshotDto.Format
            };
            var entity = new DatabaseEntity<TestScreenshot>
            {
                Data = testScreenshot,
                Id = id,
                Type = EntityType.ScreenshotType
            };
            return entity;
        }
    }
}