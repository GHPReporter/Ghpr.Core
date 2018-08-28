using System;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Helpers
{
    public class ActionHelper : IActionHelper
    {
        private readonly object _lock;
        private readonly ILogger _logger;

        public ActionHelper(ILogger logger)
        {
            _lock = new object();
            _logger = logger;
        }

        public void Simple(Action a)
        {
            try
            {
                a.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Exception($"Exception in method '{a.Method.Name}': {ex.Message}", ex);
            }
        }

        public void Safe(Action a)
        {
            lock (_lock)
            {
                Simple(a);
            }
        }
    }
}