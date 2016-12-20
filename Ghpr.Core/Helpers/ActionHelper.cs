using System;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Helpers
{
    public class ActionHelper
    {
        private readonly object _lock;
        private readonly string _outputPath;

        public ActionHelper(string outputPath)
        {
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath), "ActionHelper output must be specified!");
            }

            _outputPath = outputPath;
            _lock = new object();
        }

        public void Simple(Action a)
        {
            try
            {
                a.Invoke();
            }
            catch (Exception ex)
            {
                new Log(_outputPath).Exception(ex, $"Exception in method '{a.Method.Name}'");
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