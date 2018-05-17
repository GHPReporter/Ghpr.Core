using System;

namespace Ghpr.Core.Common
{
    public class ItemInfoDto
    {
        public Guid Guid { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}