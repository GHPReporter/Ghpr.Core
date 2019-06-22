using Ghpr.Core.Common;
using Ghpr.Core.Enums;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Common
{
    [TestFixture]
    public class TestRunDtoTests
    {
        [TestCase("passed", TestResult.Passed)]
        [TestCase("Passed", TestResult.Passed)]
        [TestCase("Passed test", TestResult.Passed)]
        [TestCase("test passed", TestResult.Passed)]
        [TestCase("error", TestResult.Broken)]
        [TestCase("ERROR", TestResult.Broken)]
        [TestCase("broken", TestResult.Broken)]
        [TestCase("Broken", TestResult.Broken)]
        [TestCase("failed", TestResult.Failed)]
        [TestCase("failure", TestResult.Failed)]
        [TestCase("Failed", TestResult.Failed)]
        [TestCase("FAILURE", TestResult.Failed)]
        [TestCase("inconclusive", TestResult.Inconclusive)]
        [TestCase("Inconclusive", TestResult.Inconclusive)]
        [TestCase("ignored", TestResult.Ignored)]
        [TestCase("skipped", TestResult.Ignored)]
        [TestCase("Ignored", TestResult.Ignored)]
        [TestCase("notexecuted", TestResult.Ignored)]
        [TestCase("as1dfd2fa4sd", TestResult.Unknown)]
        [TestCase("123", TestResult.Unknown)]
        [TestCase("!@#$%^&*(", TestResult.Unknown)]
        public void TestResultTest(string testResult, TestResult result)
        {
            var test = new TestRunDto
            {
                Result = testResult
            };
            Assert.AreEqual(result, test.TestResult);
        }
    }
}