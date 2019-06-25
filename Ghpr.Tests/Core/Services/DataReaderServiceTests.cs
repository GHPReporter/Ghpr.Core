using System;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Services;
using Ghpr.Core.Core.Settings;
using Ghpr.Core.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Tests.Core.Services
{
    [TestFixture]
    public class DataReaderServiceTests
    {
        [Test]
        public void TestCreation()
        {
            CommonCache.Instance.InitializeDataReader(new ProjectSettings(), new EmptyLogger());
            CommonCache.Instance.InitializeDataWriter(new ProjectSettings(), new EmptyLogger());
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