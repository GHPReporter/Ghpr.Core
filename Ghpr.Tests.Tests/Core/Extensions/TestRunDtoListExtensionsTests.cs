using System;
using System.Collections.Generic;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Extensions
{
    [TestFixture]
    public class TestRunDtoListExtensionsTests
    {
        [Test]
        public void TestParallelDates()
        {
            var now = DateTime.Now;
            var list = new List<KeyValuePair<TestRunDto, TestOutputDto>>
            {
                new KeyValuePair<TestRunDto, TestOutputDto>(new TestRunDto
                {
                    TestInfo = new ItemInfoDto { Start = now.AddSeconds(0), Finish = now.AddSeconds(5) }
                }, new TestOutputDto()),
                new KeyValuePair<TestRunDto, TestOutputDto>(new TestRunDto
                {
                    TestInfo = new ItemInfoDto { Start = now.AddSeconds(2), Finish = now.AddSeconds(3) }
                }, new TestOutputDto())
            };
            var start = list.GetRunStartDateTime();
            var finish = list.GetRunFinishDateTime();
            Assert.AreEqual(now.AddSeconds(0), start);
            Assert.AreEqual(now.AddSeconds(5), finish);
        }

        [Test]
        public void TestFilledDates()
        {
            var now = DateTime.Now;
            var list = new List<KeyValuePair<TestRunDto, TestOutputDto>>
            {
                new KeyValuePair<TestRunDto, TestOutputDto>(new TestRunDto
                {
                    TestInfo = new ItemInfoDto { Start = now.AddSeconds(0), Finish = now.AddSeconds(1) }
                }, new TestOutputDto()),
                new KeyValuePair<TestRunDto, TestOutputDto>(new TestRunDto
                {
                    TestInfo = new ItemInfoDto { Start = now.AddSeconds(2), Finish = now.AddSeconds(3) }
                }, new TestOutputDto())
            };
            var start = list.GetRunStartDateTime();
            var finish = list.GetRunFinishDateTime();
            Assert.AreEqual(now.AddSeconds(0), start);
            Assert.AreEqual(now.AddSeconds(3), finish);
        }
    }
}