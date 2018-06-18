using System;

namespace Ghpr.Core.Common
{
    public class ItemInfoDto
    {
        public string ItemName { get; set; }
        public Guid Guid { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}