using System;

namespace Ghpr.Core.Interfaces
{
    public interface IItemInfo
    {
        Guid Guid { get; set; }
        DateTime Start { get; set; }
        DateTime Finish { get; set; }
    }
}