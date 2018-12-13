using System;
using Ghpr.Core.Common;
using Ghpr.Core.Comparers;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core
{
    [TestFixture]
    public class SimpleItemInfoDtoComparerTests
    {
        private readonly SimpleItemInfoDtoComparer _comparer = new SimpleItemInfoDtoComparer();

        [Test]
        public void TestEmptyItems()
        {
            var dto1 = new SimpleItemInfoDto();
            var dto2 = new SimpleItemInfoDto();
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
        }

        [Test]
        public void TestNullItems()
        {
            var dto = new SimpleItemInfoDto();
            Assert.IsFalse(_comparer.Equals(null, dto));
            Assert.IsFalse(_comparer.Equals(dto, null));
        }

        [Test]
        public void TestNullItemName()
        {
            var dto1 = new SimpleItemInfoDto { ItemName = "a" };
            var dto2 = new SimpleItemInfoDto { ItemName = null };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
        }

        [Test]
        public void TestNullItemNameDifferentDates()
        {
            var now = DateTime.Now;
            var dto1 = new SimpleItemInfoDto { ItemName = null, Date = now };
            var dto2 = new SimpleItemInfoDto { ItemName = null, Date = now.AddSeconds(2) };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
        }

        [Test]
        public void TestNullItemNameSameDates()
        {
            var now = DateTime.Now;
            var dto1 = new SimpleItemInfoDto { ItemName = null, Date = now };
            var dto2 = new SimpleItemInfoDto { ItemName = null, Date = now };
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
            Assert.IsTrue(_comparer.Equals(dto2, dto1));
        }

        [Test]
        public void TestNotNullItemNameDifferentDates()
        {
            var now = DateTime.Now;
            var dto1 = new SimpleItemInfoDto { ItemName = "a", Date = now };
            var dto2 = new SimpleItemInfoDto { ItemName = "a", Date = now.AddSeconds(2) };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
        }

        [Test]
        public void TestNotNullItemNameSameDates()
        {
            var now = DateTime.Now;
            var dto1 = new SimpleItemInfoDto { ItemName = "b", Date = now };
            var dto2 = new SimpleItemInfoDto { ItemName = "b", Date = now };
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
            Assert.IsTrue(_comparer.Equals(dto2, dto1));
        }

        [Test]
        public void TestNotNullDifferentItemNameSameDates()
        {
            var now = DateTime.Now;
            var dto1 = new SimpleItemInfoDto { ItemName = "a", Date = now };
            var dto2 = new SimpleItemInfoDto { ItemName = "b", Date = now };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
        }

        [Test]
        public void TestHash()
        {
            var dto = new SimpleItemInfoDto();
            Assert.AreEqual(dto.GetHashCode(), _comparer.GetHashCode(dto));
        }
    }
}