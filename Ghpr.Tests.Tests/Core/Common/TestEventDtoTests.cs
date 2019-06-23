using System;
using Ghpr.Core.Common;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core.Common
{
    [TestFixture]
    public class TestEventDtoTests
    {
        [Test]
        public void CreateEmptyTest()
        {
            var e = new TestEventDto();
            Assert.AreEqual("", e.Comment);
            Assert.AreEqual(default(DateTime), e.Started);
            Assert.AreEqual(default(DateTime), e.Finished);
            Assert.AreEqual(null, e.EventInfo.ItemName);
            Assert.AreEqual(default(DateTime), e.EventInfo.Date);
        }

        [Test]
        public void CreateNotEmptyTest()
        {
            var now = DateTime.Now;
            var e = new TestEventDto("comment", now, now.AddSeconds(2));
            Assert.AreEqual("comment", e.Comment);
            Assert.AreEqual(now, e.Started);
            Assert.AreEqual(now.AddSeconds(2), e.Finished);
            Assert.AreEqual(null, e.EventInfo.ItemName);
            Assert.AreEqual(default(DateTime), e.EventInfo.Date);
        }
    }
}