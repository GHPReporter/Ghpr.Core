using System;
using System.IO;
using System.Threading;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class Log : ILog
    {
        private string _logFile;
        private readonly string _output;
        private readonly ActionHelper _actionHelper;
        private static readonly ReaderWriterLock Locker = new ReaderWriterLock();

        public Log(string outputPath, string logFile = "")
        {
            _output = outputPath;
            _logFile = logFile.Equals("") ? "GHPReporter.txt" : logFile;
            _actionHelper = new ActionHelper(outputPath);
        }
        
        public void WriteToFile(string msg, string fileName)
        {
            Paths.Create(_output);
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
            try
            {
                Locker.AcquireWriterLock(int.MaxValue);
                Paths.Create(_output);
                using (var sw = File.AppendText(Path.Combine(_output, _logFile)))
                {
                    try
                    {
                        var logLine = $"{DateTime.Now:G}: {msg}";
                        sw.WriteLine(logLine);
                    }
                    catch (Exception ex)
                    {
                        Exception(ex, $"Exception while logging message: '{msg}'");
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
            finally
            {
                Locker.ReleaseWriterLock();
            }
            
        }

        public void SetOutputFileName(string fileWithExtension)
        {
            _logFile = fileWithExtension;
        }

        public void Exception(Exception exception, string exceptionMessage = "")
        {
            var nl = Environment.NewLine;
            var msg = (exceptionMessage.Equals("") ? "Exception!" : exceptionMessage) + nl +
                " Message: " + nl + exception.Message+ nl +
                " Source: " + nl + exception.Source + nl +
                " StackTrace: " + nl + exception.StackTrace;
            var inner = exception.InnerException;
            while (inner != null)
            {
                msg = msg + nl + " Inner Exception: " + nl + inner.Message + nl + "StackTrace: " + nl + inner.StackTrace;
                inner = inner.InnerException;
            }
            WriteToFile(msg, $"Exception_{DateTime.Now.ToString("ddMMyy_HHmmss_fff")}_{Guid.NewGuid()}.txt");
        }
    }
}
