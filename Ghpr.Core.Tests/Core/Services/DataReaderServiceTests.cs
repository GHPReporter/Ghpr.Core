using System;
using Ghpr.Core.Common;
using Ghpr.Core.Services;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core.Services
{
    [TestFixture]
    public class DataReaderServiceTests
    {
        [Test]
        public void TestCreation()
        {
            CommonCache.Instance.InitializeDataReader(new ReporterSettings(), new EmptyLogger());
            CommonCache.Instance.InitializeDataWriter(new ReporterSettings(), new EmptyLogger());
            var reader = new DataReaderService(new MockDataReaderService(), CommonCache.Instance);
            Assert.IsInstanceOf(typeof(MockDataReaderService), reader.GetDataReader());
            Assert.DoesNotThrow(() => reader.GetTestRun(new ItemInfoDto()));
            Assert.DoesNotThrow(() => reader.GetReportSettings());
            Assert.DoesNotThrow(() => reader.GetLatestTestRun(Guid.NewGuid()));
            Assert.DoesNotThrow(() => reader.GetRun(Guid.NewGuid()));
            Assert.DoesNotThrow(() => reader.GetRunInfos());
            Assert.DoesNotThrow(() => reader.GetTestInfos(Guid.NewGuid()));
            Assert.DoesNotThrow(() => reader.GetTestOutput(new TestRunDto()));
            Assert.DoesNotThrow(() => reader.GetTestRunsFromRun(new RunDto()));
            Assert.DoesNotThrow(() => reader.GetTestScreenshots(new TestRunDto()));
            CommonCache.Instance.TearDown();
        }
    }
}