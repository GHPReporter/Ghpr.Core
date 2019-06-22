using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Utils
{
    [TestFixture]
    public class PathsTests
    {
        [Test]
        public void CreatePathsTest()
        {
            var paths = new Paths();
            Assert.NotNull(paths.File);
            Assert.NotNull(paths.Folder);
            Assert.NotNull(paths.Name);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetScreenKeyTest(int count)
        {
            Assert.AreEqual($"ghpr_screenshot_{count}", Paths.GetScreenKey(count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetTestDataDateTimeKeyTest(int count)
        {
            Assert.AreEqual($"ghpr_test_data_datetime_{count}", Paths.GetTestDataDateTimeKey(count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetTestDataCommentKeyTest(int count)
        {
            Assert.AreEqual($"ghpr_test_data_comment_{count}", Paths.GetTestDataCommentKey(count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetTestDataActualKeyTest(int count)
        {
            Assert.AreEqual($"ghpr_test_data_actual_{count}", Paths.GetTestDataActualKey(count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetTestDataExpectedKeyTest(int count)
        {
            Assert.AreEqual($"ghpr_test_data_expected_{count}", Paths.GetTestDataExpectedKey(count));
        }
    }
}