using System;
using Ghpr.Core.Common;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Common
{
    [TestFixture]
    public class TestCaseFullDtoTests
    {
        [Test]
        public void Init()
        {
            var t = new TestCaseFullDto();
            Assert.AreEqual("", t.ParentId);
            Assert.AreEqual("", t.Id);
            Assert.AreEqual("", t.GhprTestRun.Name);
            Assert.AreEqual(Guid.Empty, t.GhprTestRun.TestInfo.Guid);
            Assert.AreEqual(0, t.GhprTestScreenshots.Count);
        }

        [Test]
        public void ToStringTest()
        {
            var t = new TestCaseFullDto
            {
                ParentId = "p id",
                Id = "id"
            };
            Assert.AreEqual("GhprTestCase: Id = id, ParentID = p id", t.ToString());
        }
    }
}