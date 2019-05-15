using System;
using Ghpr.Core.Common;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core.Common
{
    [TestFixture]
    public class CommonCacheTests
    {
        [Test]
        public void InitTest()
        {
            var logger = new EmptyLogger();
            var settings = new ReporterSettings();
            var cache = CommonCache.Instance;
            Assert.IsInstanceOf(typeof(CommonCache), cache.GetDataReader());
            Assert.IsInstanceOf(typeof(CommonCache), cache.GetDataWriter());
            Assert.Throws<NullReferenceException>(() => cache.SaveTestRun(new TestRunDto(), new TestOutputDto()));
            Assert.Throws<NullReferenceException>(() => cache.GetTestRun(new ItemInfoDto()));
            Assert.Throws<NullReferenceException>(() => cache.GetReportSettings());
            Assert.Throws<NullReferenceException>(() => cache.GetLatestTestRun(Guid.NewGuid()));
            Assert.Throws<NullReferenceException>(() => cache.GetRun(Guid.NewGuid()));
            Assert.Throws<NullReferenceException>(() => cache.GetRunInfos());
            Assert.Throws<NullReferenceException>(() => cache.GetTestInfos(Guid.NewGuid()));
            Assert.Throws<NullReferenceException>(() => cache.GetTestOutput(new TestRunDto()));
            Assert.Throws<NullReferenceException>(() => cache.GetTestRunsFromRun(new RunDto()));
            Assert.Throws<NullReferenceException>(() => cache.GetTestScreenshots(new TestRunDto()));
            Assert.Throws<NullReferenceException>(() => cache.SaveReportSettings(new ReportSettingsDto(1, 2, "", "")));
            Assert.Throws<NullReferenceException>(() => cache.SaveRun(new RunDto()));
            var scr = new TestScreenshotDto
            {
                Base64Data = "adfas",
                Format = "png",
                TestGuid = Guid.NewGuid(),
                TestScreenshotInfo = new SimpleItemInfoDto {Date = DateTime.Now, ItemName = "item"}
            };
            Assert.Throws<NullReferenceException>(() => cache.SaveScreenshot(scr));
            Assert.Throws<NullReferenceException>(() => cache.UpdateTestOutput(new ItemInfoDto(), new TestOutputDto()));
            Assert.Throws<NullReferenceException>(() => cache.DeleteRun(new ItemInfoDto()));
            Assert.Throws<NullReferenceException>(() => cache.DeleteTest(new TestRunDto()));
            Assert.Throws<NullReferenceException>(() => cache.DeleteTestOutput(new TestRunDto(), new TestOutputDto()));
            Assert.Throws<NullReferenceException>(() => cache.DeleteTestScreenshot(new TestRunDto(), new TestScreenshotDto()));
            cache.InitializeDataWriter(settings.DefaultSettings, logger);
            cache.InitializeDataReader(settings.DefaultSettings, logger);
            Assert.IsInstanceOf(typeof(CommonCache), cache.GetDataReader());
            Assert.IsInstanceOf(typeof(CommonCache), cache.GetDataWriter());
            Assert.DoesNotThrow(() => cache.SaveTestRun(new TestRunDto(), new TestOutputDto()));
            Assert.DoesNotThrow(() => cache.GetTestRun(new ItemInfoDto()));
            Assert.DoesNotThrow(() => cache.GetReportSettings());
            Assert.DoesNotThrow(() => cache.GetLatestTestRun(Guid.NewGuid()));
            Assert.DoesNotThrow(() => cache.GetRun(Guid.NewGuid()));
            Assert.DoesNotThrow(() => cache.GetRunInfos());
            Assert.DoesNotThrow(() => cache.GetTestInfos(Guid.NewGuid()));
            Assert.DoesNotThrow(() => cache.GetTestOutput(new TestRunDto()));
            Assert.DoesNotThrow(() => cache.GetTestRunsFromRun(new RunDto()));
            Assert.DoesNotThrow(() => cache.GetTestScreenshots(new TestRunDto()));
            Assert.DoesNotThrow(() => cache.SaveReportSettings(new ReportSettingsDto(1, 2, "", "")));
            Assert.DoesNotThrow(() => cache.SaveRun(new RunDto()));
            Assert.DoesNotThrow(() => cache.SaveScreenshot(scr));
            Assert.DoesNotThrow(() => cache.UpdateTestOutput(new ItemInfoDto(), new TestOutputDto()));
            Assert.DoesNotThrow(() => cache.DeleteRun(new ItemInfoDto()));
            Assert.DoesNotThrow(() => cache.DeleteTest(new TestRunDto()));
            Assert.DoesNotThrow(() => cache.DeleteTestOutput(new TestRunDto(), new TestOutputDto()));
            Assert.DoesNotThrow(() => cache.DeleteTestScreenshot(new TestRunDto(), scr));
            cache.TearDown();
        }
    }
}