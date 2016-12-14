using System;
using System.IO;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class Log : ILog
    {
        public Log(string outputPath, string logFile = "")
        {
            _output = outputPath;
            _logFile = logFile.Equals("") ? "GHPReporter.txt" : logFile;
        }

        private string _logFile;
        private readonly string _output;

        public void WriteToFile(string msg, string fileName)
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

        public void Write(string msg)
        {
            Directory.CreateDirectory(_output);
            using (var sw = File.AppendText(Path.Combine(_output, _logFile)))
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

        public void SetOutputFileName(string fileWithExtension)
        {
            _logFile = fileWithExtension;
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
            WriteToFile(msg, "Exception_" + DateTime.Now.ToString("ddMMyy_HHmmss_fff") + ".txt");
        }
    }
}
