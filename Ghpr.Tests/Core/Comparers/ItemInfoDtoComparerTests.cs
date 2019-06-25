using System;
using Ghpr.Core.Core.Common;
using Ghpr.Core.Core.Comparers;
using NUnit.Framework;

namespace Ghpr.Tests.Core.Comparers
{
    [TestFixture]
    public class ItemInfoDtoComparerTests
    {
        private readonly ItemInfoDtoComparer _comparer = new ItemInfoDtoComparer();

        [Test]
        public void TestNull()
        {
            Assert.IsFalse(_comparer.Equals(null, new ItemInfoDto()));
            Assert.IsFalse(_comparer.Equals(new ItemInfoDto(), null));
            Assert.IsTrue(_comparer.Equals(null, null));
        }

        [Test]
        public void AreEqualTest()
        {
            var now = DateTime.Now;
            var guid1 = Guid.NewGuid();
            var dto1 = new ItemInfoDto();
            var dto2 = new ItemInfoDto();
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
            dto1.Guid = guid1;
            dto2.Guid = guid1;
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
            dto1.ItemName = "a1";
            dto2.ItemName = "a2";
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
            dto1.Start = now;
            dto2.Start = now;
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
            dto1.Finish = now.AddSeconds(1);
            dto2.Finish = now.AddSeconds(1);
            Assert.IsTrue(_comparer.Equals(dto1, dto2));
        }

        [Test]
        public void AreNotEqualTest()
        {
            var now = DateTime.Now;
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var dto1 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now, Finish = now.AddSeconds(1) };
            var dto2 = new ItemInfoDto { Guid = guid2, ItemName = "a1", Start = now, Finish = now.AddSeconds(1) };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
            dto1 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now.AddSeconds(2), Finish = now.AddSeconds(1) };
            dto2 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now, Finish = now.AddSeconds(1) };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
            dto1 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now, Finish = now.AddSeconds(1) };
            dto2 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now, Finish = now.AddSeconds(2) };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
            dto1 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now.AddSeconds(2), Finish = now.AddSeconds(1) };
            dto2 = new ItemInfoDto { Guid = guid1, ItemName = "a1", Start = now, Finish = now.AddSeconds(2) };
            Assert.IsFalse(_comparer.Equals(dto1, dto2));
            Assert.IsFalse(_comparer.Equals(dto2, dto1));
        }

        [Test]
        public void HashCodeTest()
        {
            var now = DateTime.Now;
            var guid = Guid.NewGuid();
            var dto = new ItemInfoDto { Guid = guid, ItemName = "a1", Start = now, Finish = now.AddSeconds(1) };
            Assert.AreEqual(dto.GetHashCode(), _comparer.GetHashCode(dto));
        }
    }
}