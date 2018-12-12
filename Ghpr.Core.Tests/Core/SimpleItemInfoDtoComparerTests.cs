using Ghpr.Core.Common;
using Ghpr.Core.Comparers;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core
{
    [TestFixture]
    public class SimpleItemInfoDtoComparerTests
    {
        [Test]
        public void TestEmptyItems()
        {
            var dto1 = new SimpleItemInfoDto();
            var dto2 = new SimpleItemInfoDto();
            var comparer = new SimpleItemInfoDtoComparer();
            var res = comparer.Equals(dto1, dto2);
            Assert.IsTrue(res);
        }

        [Test]
        public void TestHash()
        {
            var dto = new SimpleItemInfoDto();
            var comparer = new SimpleItemInfoDtoComparer();
            Assert.AreEqual(dto.GetHashCode(), comparer.GetHashCode(dto));
        }
    }
}