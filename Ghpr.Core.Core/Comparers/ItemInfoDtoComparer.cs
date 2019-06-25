using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Comparers
{
    public class ItemInfoDtoComparer : IEqualityComparer<ItemInfoDto>
    {
        public bool Equals(ItemInfoDto x, ItemInfoDto y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.Guid.Equals(y.Guid) && 
                x.Start.Equals(y.Start) && 
                x.Finish.Equals(y.Finish);
        }

        public int GetHashCode(ItemInfoDto obj)
        {
            return obj.GetHashCode();
        }
    }
}