using System.Collections.Generic;
using Ghpr.Core.Core.Common;

namespace Ghpr.Core.Core.Comparers
{
    public class SimpleItemInfoDtoComparer : IEqualityComparer<SimpleItemInfoDto>
    {
        public bool Equals(SimpleItemInfoDto x, SimpleItemInfoDto y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            if (x.ItemName == null && y.ItemName != null)
            {
                return false;
            }
            if (x.ItemName != null && y.ItemName == null)
            {
                return false;
            }
            var datesEqual = x.Date.Equals(y.Date);
            bool namesEqual = false;
            if (x.ItemName == null && y.ItemName == null)
            {
                namesEqual = true;
            }
            else if (x.ItemName != null && y.ItemName != null)
            {
                namesEqual = x.ItemName.Equals(y.ItemName);
            }

            return  datesEqual && namesEqual;
        }

        public int GetHashCode(SimpleItemInfoDto obj)
        {
            return obj.GetHashCode();
        }
    }
}