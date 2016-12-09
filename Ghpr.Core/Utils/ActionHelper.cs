using System;

namespace Ghpr.Core.Utils
{
    public class ActionHelper
    {
        private readonly object _lock;
        private readonly Log _log;

        public ActionHelper(string exceptionOutputPath)
        {
            _log = new Log(exceptionOutputPath);
            _lock = new object();
        }

        public void Safe(Action a)
        {
            lock (_lock)
            {
                try
                {
                    a.Invoke();
                }
                catch (Exception ex)
                {
                    _log.Exception(ex, $"Exception in method '{a.Method.Name}'");
                }
            }
        }
    }
}