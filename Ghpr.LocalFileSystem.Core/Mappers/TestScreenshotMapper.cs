using Ghpr.Core.Core.Common;
using Ghpr.LocalFileSystem.Core.Entities;
using Ghpr.LocalFileSystem.Core.Providers;

namespace Ghpr.LocalFileSystem.Core.Mappers
{
    public static class TestScreenshotMapper
    {
        public static TestScreenshot Map(this TestScreenshotDto testScreenshotDto)
        {
            var name = NamesProvider.GetScreenshotFileName(testScreenshotDto.TestScreenshotInfo.Date);
            var testScreenshot = new TestScreenshot
            {
                TestScreenshotInfo = testScreenshotDto.TestScreenshotInfo.MapSimpleItemInfo(name),
                Base64Data = testScreenshotDto.Base64Data,
                TestGuid = testScreenshotDto.TestGuid,
                Format = testScreenshotDto.Format
            };
            return testScreenshot;
        }

        public static TestScreenshotDto ToDto(this TestScreenshot testScreenshot)
        {
            var testScreenshotDto = new TestScreenshotDto
            {
                TestScreenshotInfo = testScreenshot.TestScreenshotInfo.ToDto(),
                Base64Data = testScreenshot.Base64Data,
                TestGuid = testScreenshot.TestGuid,
                Format = testScreenshot.Format
            };
            return testScreenshotDto;
        }
    }
}