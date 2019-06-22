using Ghpr.Core.Helpers;
using Ghpr.Core.Processors;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Tests.Tests.Core.Processors
{
    [TestFixture]
    public class ReportCleanUpProcessorTests
    {
        [Test]
        public void Test()
        {
            var p = new ReportCleanUpProcessor(new EmptyLogger(), new ActionHelper(new EmptyLogger()));
            p.CleanUpReport(new RetentionSettings(), new MockDataReaderService(), new MockDataWriterService());
        }
    }
}