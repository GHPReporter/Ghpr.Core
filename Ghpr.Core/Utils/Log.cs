using System;
using System.IO;

namespace Ghpr.Core.Utils
{
    public class Log
    {
        public Log(string outputPath)
        {
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath), "Log output must be specified!");
            }
            _output = outputPath;
        }

        private const string LogFile = @"GHPReporter.txt";
        private readonly string _output;

        private void WriteToFile(string msg, string fileName)
        {
            Directory.CreateDirectory(_output);
            using (var sw = File.AppendText(Path.Combine(_output, fileName)))
            {
                try
                {
                    var logLine = $"{DateTime.Now:G}: {msg}";
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        private void WriteToFile(string msg, string fileName, string filePath)
        {
            Directory.CreateDirectory(_output);
            using (var sw = File.AppendText(Path.Combine(filePath, fileName)))
            {
                try
                {
                    var logLine = $"{DateTime.Now:G}: {msg}";
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        public void Write(string msg)
        {
            Directory.CreateDirectory(_output);
            using (var sw = File.AppendText(Path.Combine(_output, LogFile)))
            {
                try
                {
                    var logLine = $"{DateTime.Now:G}: {msg}";
                    sw.WriteLine(logLine);
                }
                catch (Exception ex)
                {
                    Exception(ex);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        public void Exception(Exception exception, string exceptionMessage = "")
        {
            var msg = (exceptionMessage.Equals("") ? "Exception!" : exceptionMessage) + Environment.NewLine
                + " Message: " + Environment.NewLine + exception.Message + Environment.NewLine +
                "StackTrace: " + Environment.NewLine + exception.StackTrace;
            var inner = exception.InnerException;
            while (inner != null)
            {
                msg = msg + Environment.NewLine + " Inner Exception: " + Environment.NewLine +
                    inner.Message + Environment.NewLine +
                "StackTrace: " + Environment.NewLine + inner.StackTrace;
                inner = inner.InnerException;
            }
            WriteToFile(msg, "Exception_" + DateTime.Now.ToString("ddMMyyHHmmssfff") + ".txt");
        }

        public void Exception(Exception exception, string path, string exceptionMessage)
        {
            var msg = exceptionMessage + Environment.NewLine
                + " Message: " + Environment.NewLine + exception.Message + Environment.NewLine +
                "StackTrace: " + Environment.NewLine + exception.StackTrace;
            WriteToFile(msg, "Exception_" + DateTime.Now.ToString("ddMMyyHHmmssfff") + ".txt", path);
        }

        public void Warning(string warningMessage)
        {
            WriteToFile(warningMessage, "Warning_" + DateTime.Now.ToString("ddMMyyHHmmssfff") + ".txt");
        }
    }
}
