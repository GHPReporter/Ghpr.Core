using System;
using Ghpr.Core.Providers;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core.Providers
{
    [TestFixture]
    public class NamesProviderTests
    {
        [Test]
        public void TestFileName()
        {
            var finishDateTime = new DateTime(2018, 10, 25, 5, 45, 55);
            var testFileName = NamesProvider.GetTestRunFileName(finishDateTime);
            Assert.AreEqual($"test_{finishDateTime:yyyyMMdd_HHmmssfff}.json", testFileName);
        }

        [Test]
        public void GetTestOutputFileName()
        {
            var finishDateTime = new DateTime(2018, 10, 25, 5, 45, 55);
            var testOutputFileName = NamesProvider.GetTestOutputFileName(finishDateTime);
            Assert.AreEqual($"test_output_{finishDateTime:yyyyMMdd_HHmmssfff}.json", testOutputFileName);
        }

        [Test]
        public void GetRunFileName()
        {
            var runGuid = Guid.NewGuid();
            var runFileName = NamesProvider.GetRunFileName(runGuid);
            Assert.AreEqual($"run_{runGuid.ToString("D").ToLower()}.json", runFileName);
        }

        [Test]
        public void GetScreenshotFileName()
        {
            var creationDate = new DateTime(2018, 10, 25, 5, 45, 55);
            var screenshotFileName = NamesProvider.GetScreenshotFileName(creationDate);
            Assert.AreEqual($"img_{creationDate:yyyyMMdd_HHmmssfff}.json", screenshotFileName);
        }
    }
}