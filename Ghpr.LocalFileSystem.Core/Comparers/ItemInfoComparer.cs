using System.Collections.Generic;
using Ghpr.LocalFileSystem.Core.Entities;

namespace Ghpr.LocalFileSystem.Core.Comparers
{
    public class ItemInfoComparer : IEqualityComparer<ItemInfo>
    {
        public bool Equals(ItemInfo x, ItemInfo y)
        {
            return x.Guid.Equals(y.Guid) && 
                x.Start.Equals(y.Start) &&
                x.Finish.Equals(y.Finish);
        }

        public int GetHashCode(ItemInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}