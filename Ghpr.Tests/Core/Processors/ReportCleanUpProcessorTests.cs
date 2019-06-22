using Ghpr.Core.Core.Helpers;
using Ghpr.Core.Core.Processors;
using Ghpr.Core.Core.Settings;
using Ghpr.Core.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Tests.Core.Processors
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