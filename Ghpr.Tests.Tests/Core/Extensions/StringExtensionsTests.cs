using System;
using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.Core.Settings;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase(@"C:\SomePath")]
        [TestCase(@"C:\SomePath\1")]
        [TestCase(@"C:\SomePath\1\folder")]
        [TestCase(@"C:\SomePath\folder\folder")]
        public void CreatePathTest(string path)
        {
            path.Create();
            Assert.IsTrue(Directory.Exists(path));
            Directory.Delete(path, true);
        }

        [TestCase(null, "d98c1dd4-008f-04b2-e980-0998ecf8427e")]
        [TestCase("value", "60c16320-6e8d-af0b-8024-9c42e2be5804")]
        [TestCase("@#$%^&*", "13f43c69-dfa2-20cc-d646-f6640cc49968")]
        [TestCase("value  -  asdfa - s!", "5148c927-4668-af01-b893-28f44bb86689")]
        [TestCase("va1234lue", "777fde87-a8f2-2645-4e74-b51ff7925b8c")]
        [TestCase("valu$%^&e", "e6c3ff61-6b8d-c229-c193-f24ac13a1452")]
        [TestCase("", "d98c1dd4-008f-04b2-e980-0998ecf8427e")]
        [TestCase("   ", "f0318662-2173-2db2-8c17-6c200c855e1b")]
        public void Md5HashGuidTest(string value, string guid)
        {
            Assert.AreEqual(Guid.Parse(guid), value.ToMd5HashGuid());
        }

        [Test]
        public void LoadAsTest()
        {
            var uri = new Uri(typeof(ReporterSettings).Assembly.CodeBase);
            var settingsPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "", "Ghpr.Core.Settings.json");
            var s = settingsPath.LoadAs<ReporterSettings>();
            Assert.AreEqual("C:\\_GHPReporter_Core_Report", s.DefaultSettings.OutputPath);
        }
    }
}