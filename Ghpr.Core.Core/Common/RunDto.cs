using System.Collections.Generic;

namespace Ghpr.Core.Core.Common
{
    public class RunDto
    {
        public List<ItemInfoDto> TestsInfo { get; set; }
        public ItemInfoDto RunInfo { get; set; }
        public string Sprint { get; set; }
        public string Name { get; set; }
        public RunSummaryDto RunSummary { get; set; }
    }
}