using System;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Factories;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core
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


        [Test]
        public void LoggerTest()
        {
            var e = new Exception();
            var o = new object();
            var r = ReporterFactory.Build(new MockTestDataProvider());
            Assert.DoesNotThrow(() => { r.Logger.SetUp(r.ReporterSettings); });
            Assert.DoesNotThrow(() => { r.Logger.Info(o, e); });
            Assert.DoesNotThrow(() => { r.Logger.Info("msg", e); });
            Assert.DoesNotThrow(() => { r.Logger.Info("msg"); });
            Assert.DoesNotThrow(() => { r.Logger.Warn(o, e); });
            Assert.DoesNotThrow(() => { r.Logger.Warn("msg", e); });
            Assert.DoesNotThrow(() => { r.Logger.Warn("msg"); });
            Assert.DoesNotThrow(() => { r.Logger.Error(o, e); });
            Assert.DoesNotThrow(() => { r.Logger.Error("msg", e); });
            Assert.DoesNotThrow(() => { r.Logger.Error("msg"); });
            Assert.DoesNotThrow(() => { r.Logger.Debug(o, e); });
            Assert.DoesNotThrow(() => { r.Logger.Debug("msg", e); });
            Assert.DoesNotThrow(() => { r.Logger.Debug("msg"); });
            Assert.DoesNotThrow(() => { r.Logger.Fatal(o, e); });
            Assert.DoesNotThrow(() => { r.Logger.Fatal("msg", e); });
            Assert.DoesNotThrow(() => { r.Logger.Fatal("msg"); });
            Assert.DoesNotThrow(() => { r.Logger.Exception(o, e); });
            Assert.DoesNotThrow(() => { r.Logger.Exception("msg", e); });
            Assert.DoesNotThrow(() => { r.Logger.Exception("msg"); });
            Assert.DoesNotThrow(() => { r.Logger.TearDown(); });
        }
    }
}