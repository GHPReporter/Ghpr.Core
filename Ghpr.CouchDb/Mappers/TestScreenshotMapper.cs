using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class TestScreenshotMapper
    {
        public static DatabaseEntity<TestScreenshot> Map(this TestScreenshotDto testScreenshotDto)
        {
            var testScreenshot = new TestScreenshot
            {
                Date = testScreenshotDto.Date,
                Base64Data = testScreenshotDto.Base64Data,
                TestGuid = testScreenshotDto.TestGuid,
                Format = testScreenshotDto.Format
            };
            var entity = new DatabaseEntity<TestScreenshot>
            {
                Data = testScreenshot,
                Id = $"screenshot-{testScreenshot.TestGuid.ToString()}-{testScreenshot.Date:yyyyMMdd_HHmmssfff}",
                Type = EntityType.ScreenshotType
            };
            return entity;
        }
    }
}