using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class TestScreenshotMapper
    {
        public static TestScreenshot Map(this TestScreenshotDto testScreenshotDto)
        {
            var testScreenshot = new TestScreenshot
            {
                Date = testScreenshotDto.Date,
                Data = testScreenshotDto.Data,
                TestGuid = testScreenshotDto.TestGuid
            };
            return testScreenshot;
        }
    }
}