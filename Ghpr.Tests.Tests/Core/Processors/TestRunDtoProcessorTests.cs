using System;
using System.Collections.Generic;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Extensions;
using Ghpr.Core.Core.Processors;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Processors
{
    [TestFixture]
    public class TestRunDtoProcessorTests
    {
        [Test]
        public void TestProcessRunGuid()
        {
            var guid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto();
            var testOnFinish = new TestRunDto();
            var finalTest = p.Process(testOnStart, testOnFinish, guid);
            Assert.AreEqual(guid, finalTest.RunGuid);
        }

        [Test]
        public void TestInfoTest1()
        {
            var now = DateTime.Now;
            var runGuid = Guid.NewGuid();
            var testGuid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto(testGuid, "Cool Test")
            {
                TestInfo = new ItemInfoDto
                {
                    Start = now,
                    Finish = now.AddSeconds(2),
                    ItemName = "item1"
                }
            };
            var testOnFinish = new TestRunDto
            {
                TestInfo = new ItemInfoDto
                {
                    Start = now.AddSeconds(4),
                    Finish = now.AddSeconds(6),
                    ItemName = "item2"
                }
            };
            var finalTest = p.Process(testOnStart, testOnFinish, runGuid);
            Assert.AreEqual(testOnStart.TestInfo.Start, finalTest.TestInfo.Start);
            Assert.AreEqual(testOnFinish.TestInfo.Finish, finalTest.TestInfo.Finish);
            Assert.AreEqual(testOnFinish.TestInfo.ItemName, finalTest.TestInfo.ItemName);
        }

        [Test]
        public void TestInfoTest2()
        {
            var now = DateTime.Now.AddSeconds(-3);
            var runGuid = Guid.NewGuid();
            var testGuid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto(testGuid, "Cool Test")
            {
                TestInfo = new ItemInfoDto
                {
                    ItemName = "item1"
                }
            };
            var testOnFinish = new TestRunDto
            {
                TestInfo = new ItemInfoDto
                {
                    Start = now.AddSeconds(4)
                }
            };
            var finalTest = p.Process(testOnStart, testOnFinish, runGuid);
            Assert.AreEqual(testOnFinish.TestInfo.Start, finalTest.TestInfo.Start);
            Assert.True(finalTest.TestInfo.Finish > now);
        }

        [Test]
        public void TestGuidTakenFromFullName()
        {
            var runGuid = Guid.NewGuid();
            var testGuid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto(testGuid, "Cool Test");
            var testOnFinish = new TestRunDto
            {
                FullName = "Cool test full name"
            };
            var finalTest = p.Process(testOnStart, testOnFinish, runGuid);
            Assert.AreEqual("Cool test full name".ToMd5HashGuid(), finalTest.TestInfo.Guid);
        }

        [Test]
        public void TestEvents()
        {
            var runGuid = Guid.NewGuid();
            var testGuid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto(testGuid, "Cool Test")
            {
                Events = new List<TestEventDto>
                {
                    new TestEventDto("ev1")
                    {
                        Comment = "comment",
                        EventInfo = new SimpleItemInfoDto { Date = DateTime.Now, ItemName = "item" },
                        Finished = DateTime.Now,
                        Started = DateTime.Now.AddSeconds(-3)
                    },
                    new TestEventDto("ev2")
                }
            };
            var testOnFinish = new TestRunDto
            {
                Events = new List<TestEventDto> { new TestEventDto("ev3"), new TestEventDto("ev4") }
            };
            var finalTest = p.Process(testOnStart, testOnFinish, runGuid);
            Assert.AreEqual(0, finalTest.TestData.Count);
            Assert.AreEqual(0, finalTest.Screenshots.Count);
            Assert.AreEqual(4, finalTest.Events.Count);
        }

        [Test]
        public void TestData()
        {
            var now = DateTime.Now;
            var runGuid = Guid.NewGuid();
            var testGuid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto(testGuid, "Cool Test")
            {
                TestData = new List<TestDataDto>
                {
                    new TestDataDto
                    {
                        Actual = "a1",
                        Expected = "e1",
                        Comment = "c1",
                        TestDataInfo = new SimpleItemInfoDto{Date = now}
                    },
                    new TestDataDto
                    {
                        Actual = "a2",
                        Expected = "e2",
                        Comment = "c2",
                        TestDataInfo = new SimpleItemInfoDto{Date = now.AddSeconds(1)}
                    }
                }
            };
            var testOnFinish = new TestRunDto
            {
                TestData = new List<TestDataDto>
                {
                    new TestDataDto
                    {
                        Actual = "a3",
                        Expected = "e3",
                        Comment = "c3",
                        TestDataInfo = new SimpleItemInfoDto{Date = now.AddSeconds(2)}
                    },
                    new TestDataDto
                    {
                        Actual = "a4",
                        Expected = "e4",
                        Comment = "c4",
                        TestDataInfo = new SimpleItemInfoDto{Date = now.AddSeconds(3)}
                    }
                }
            };
            var finalTest = p.Process(testOnStart, testOnFinish, runGuid);
            Assert.AreEqual(4, finalTest.TestData.Count);
            Assert.AreEqual(0, finalTest.Screenshots.Count);
            Assert.AreEqual(0, finalTest.Events.Count);
        }

        [Test]
        public void TestScreenshots()
        {
            var now = DateTime.Now;
            var runGuid = Guid.NewGuid();
            var testGuid = Guid.NewGuid();
            var p = new TestRunDtoProcessor();
            var testOnStart = new TestRunDto(testGuid, "Cool Test")
            {
                Screenshots = new List<SimpleItemInfoDto>
                {
                    new SimpleItemInfoDto{Date = now.AddSeconds(0)},
                    new SimpleItemInfoDto{Date = now.AddSeconds(1)}
                }
            };
            var testOnFinish = new TestRunDto
            {
                Screenshots = new List<SimpleItemInfoDto>
                {
                    new SimpleItemInfoDto{Date = now.AddSeconds(2)},
                    new SimpleItemInfoDto{Date = now.AddSeconds(3)}
                }
            };
            var finalTest = p.Process(testOnStart, testOnFinish, runGuid);
            Assert.AreEqual(0, finalTest.TestData.Count);
            Assert.AreEqual(4, finalTest.Screenshots.Count);
            Assert.AreEqual(0, finalTest.Events.Count);
        }
    }
}