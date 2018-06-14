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
                Name = LocationsProvider.GetScreenshotFileName(testScreenshotDto.Date, testScreenshotDto.Format),
                Base64Data = testScreenshotDto.Base64Data,
                TestGuid = testScreenshotDto.TestGuid,
                Format = testScreenshotDto.Format
            };
            return testScreenshot;
        }
    }
}