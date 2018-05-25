using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Mappers
{
    public static class TestScreenshotMapper
    {
        public static TestScreenshot Map(this TestScreenshotDto testScreenshotDto)
        {
            var testScreenshot = new TestScreenshot
            {
                Date = testScreenshotDto.Date,
                Name = LocationsProvider.GetScreenshotFileName(testScreenshotDto.Date),
                Data = testScreenshotDto.Data,
                TestGuid = testScreenshotDto.TestGuid
            };
            return testScreenshot;
        }
    }
}