using Ghpr.Core.Common;
using Ghpr.Core.Factories;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core
{
    [TestFixture]
    public class ReporterTests
    {
        [Test]
        public void TestReportProcess()
        {
            var r = ReporterFactory.Build(new MockTestDataProvider());
            Assert.IsFalse(r.TestRunStarted);
            r.RunStarted();
            Assert.IsTrue(r.TestRunStarted);
            Assert.AreEqual(0, r.RunRepository.CurrentRun.TestsInfo.Count);
            Assert.AreEqual(0, r.RunRepository.CurrentRun.RunSummary.Total);
            Assert.AreEqual("", r.RunRepository.CurrentRun.Name);
            r.SetRunName("new name");
            Assert.AreEqual("new name", r.RunRepository.CurrentRun.Name);
            r.TestStarted(new TestRunDto());
            r.TestFinished(new TestRunDto(), new TestOutputDto());
            r.AddCompleteTestRun(new TestRunDto(), new TestOutputDto());
            r.RunFinished();
            r.TearDown();
        }

        [Test]
        public void TestSetTestDataProvider()
        {
            var r = ReporterFactory.Build(new MockTestDataProvider());
            Assert.IsInstanceOf(typeof(MockTestDataProvider), r.TestDataProvider);
            r.SetTestDataProvider(new MockTestDataProviderWithException());
            Assert.IsInstanceOf(typeof(MockTestDataProviderWithException), r.TestDataProvider);
        }
    }
}