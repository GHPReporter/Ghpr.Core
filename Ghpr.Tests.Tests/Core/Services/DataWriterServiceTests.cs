using System;
using Ghpr.Core.Common;
using Ghpr.Core.Services;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Services
{
    [TestFixture]
    public class DataWriterServiceTests
    {
        [Test]
        public void TestCreation()
        {
            CommonCache.Instance.InitializeDataReader(new ProjectSettings(), new EmptyLogger());
            CommonCache.Instance.InitializeDataWriter(new ProjectSettings(), new EmptyLogger());
            var writer = new DataWriterService(new MockDataWriterService(), CommonCache.Instance);
            Assert.IsInstanceOf(typeof(MockDataWriterService), writer.GetDataWriter());
            Assert.DoesNotThrow(() => writer.SaveReportSettings(new ReportSettingsDto(1, 2, "", "")));
            Assert.DoesNotThrow(() => writer.SaveTestRun(new TestRunDto(), new TestOutputDto()));
            Assert.DoesNotThrow(() => writer.SaveReportSettings(new ReportSettingsDto(1, 2, "", "")));
            Assert.DoesNotThrow(() => writer.SaveRun(new RunDto()));
            var scr = new TestScreenshotDto
            {
                Base64Data = "adfas",
                Format = "png",
                TestGuid = Guid.NewGuid(),
                TestScreenshotInfo = new SimpleItemInfoDto {Date = DateTime.Now, ItemName = "item"}
            };
            Assert.DoesNotThrow(() => writer.SaveScreenshot(scr));
            Assert.DoesNotThrow(() => writer.UpdateTestOutput(new ItemInfoDto(), new TestOutputDto()));
            Assert.DoesNotThrow(() => writer.DeleteRun(new ItemInfoDto()));
            Assert.DoesNotThrow(() => writer.DeleteTest(new TestRunDto()));
            Assert.DoesNotThrow(() => writer.DeleteTestOutput(new TestRunDto(), new TestOutputDto()));
            Assert.DoesNotThrow(() => writer.DeleteTestScreenshot(new TestRunDto(), scr));
            CommonCache.Instance.TearDown();
        }
    }
}