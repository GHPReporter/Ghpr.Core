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
                Data = testScreenshotDto.Data,
                TestGuid = testScreenshotDto.TestGuid
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