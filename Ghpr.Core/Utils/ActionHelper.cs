using System;

namespace Ghpr.Core.Utils
{
    public class ActionHelper
    {
        private readonly object _lock;
        private readonly Log _log;

        public ActionHelper(string outputPath)
        {
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath), "ActionHelper output must be specified!");
            }

            _log = new Log(outputPath);
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