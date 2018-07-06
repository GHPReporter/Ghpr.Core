using System.Collections.Generic;
using Ghpr.Core.Common;

namespace Ghpr.Core.Comparers
{
    public class SimpleItemInfoDtoComparer : IEqualityComparer<SimpleItemInfoDto>
    {
        public bool Equals(SimpleItemInfoDto x, SimpleItemInfoDto y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y== null)
            {
                return false;
            }
            return x.Date.Equals(y.Date) && x.ItemName.Equals(y.ItemName);
        }

        public int GetHashCode(SimpleItemInfoDto obj)
        {
            return obj.GetHashCode();
        }
    }
}