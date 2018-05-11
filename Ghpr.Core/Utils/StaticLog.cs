using System.Collections.Generic;
using System.Linq;

namespace Ghpr.Core.Utils
{
    public static class StaticLog
    {
        private static object _lock;
        private static List<Log> _logs;
        private static string _output;

        public static void Initialize(string outputPath)
        {
            _lock = new object();
            _output = outputPath;
            _logs = new List<Log>();
            var log = new Log(outputPath);
            _logs.Add(log);
        }

        public static Log Logger(string fileName = "")
        {
            lock (_lock)
            {
                if (fileName.Equals(""))
                {
                    fileName = Log.DefaultLog;
                }
                if (!_logs.Any(log => log.LogFile.Equals(fileName)))
                {
                    _logs.Add(new Log(_output, fileName));
                }
                return _logs.First(log => log.LogFile.Equals(fileName));
            }
        }
    }
}