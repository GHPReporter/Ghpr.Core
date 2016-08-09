using System;

namespace Ghpr.Core.Utils
{
    public static class ActionHelper
    {
        public static void SafeAction(Action a)
        {
            try
            {
                a.Invoke();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, $"Exception in method '{a.Method.Name}'");
            }
        }
    }
}