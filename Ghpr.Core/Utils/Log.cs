using System;
using System.IO;
using System.Threading;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Utils
{
    public class Log : ILog
    {
        public const string DefaultLog = "GHPReporter.txt";
        public string LogFile { get; private set; }
        public string Output { get; }
        private static readonly ReaderWriterLock Locker = new ReaderWriterLock();

        public Log(string outputPath, string logFile = "")
        {
            Output = outputPath;
            LogFile = logFile.Equals("") ? DefaultLog : logFile;
        }
        
        public void WriteToFile(string msg, string fileName)
        {
            Output.Create();
            using (var sw = File.AppendText(Path.Combine(Output, fileName)))
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
                Output.Create();
                using (var sw = File.AppendText(Path.Combine(Output, LogFile)))
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
            LogFile = fileWithExtension;
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
            WriteToFile(msg, $"Exception_{DateTime.Now:ddMMyy_HHmmss_fff}_{Guid.NewGuid()}.txt");
        }
    }
}
