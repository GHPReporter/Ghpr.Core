using System;

namespace Ghpr.Core.Utils
{
    public class ActionHelper
    {
        private readonly Log _log;

        public ActionHelper(string exceptionOutputPath)
        {
            _log = new Log(exceptionOutputPath);
        }

        public void Safe(Action a)
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