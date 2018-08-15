using System.Collections.Generic;

namespace Ghpr.Core.Common
{
    public class TestCaseFullDto
    {
        public TestCaseFullDto()
        {
            Id = "";
            ParentId = "";
            GhprTestRun = new TestRunDto();
            GhprTestOutput = new TestOutputDto();
            GhprTestScreenshots = new List<TestScreenshotDto>();
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public TestRunDto GhprTestRun { get; set; }
        public TestOutputDto GhprTestOutput { get; set; }
        public List<TestScreenshotDto> GhprTestScreenshots { get; set; }

        public override string ToString()
        {
            return $"GhprTestCase: Id = {Id}, ParentID = {ParentId}";
        }
    }
}