using System;

namespace Ghpr.Core.Interfaces
{
    public interface IActionHelper
    {
        void Simple(Action a);
        void Safe(Action a);
    }
}