using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Comparers
{
    public class ItemInfoComparer : IEqualityComparer<ItemInfo>
    {
        public bool Equals(ItemInfo x, ItemInfo y)
        {
            return x.Guid.Equals(y.Guid) && 
                x.Start.Equals(y.Start) && 
                x.Finish.Equals(y.Finish) &&
                x.FileName.Equals(y.FileName);
        }

        public int GetHashCode(ItemInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}