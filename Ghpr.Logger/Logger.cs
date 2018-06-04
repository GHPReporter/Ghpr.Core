using System;
using System.IO;
using System.Threading;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Logger
{
    public class Logger : ILogger
    {
        private readonly string _logFileName;
        private readonly string _outputPath;
        private static readonly ReaderWriterLock Locker = new ReaderWriterLock();

        private const string InfoLevel = "INFO";
        private const string WarningLevel = "WARN";
        private const string DebugLevel = "DEBUG";
        private const string FatalLevel = "FATAL";
        private const string ErrorLevel = "ERROR";
        private const string ExceptionLevel = "EXCEPTION";

        public Logger(string outputPath, string logFile)
        {
            _outputPath = outputPath;
            _logFileName = logFile;
        }

        private void Write(string msg, string logLevel)
        {
            try
            {
                Locker.AcquireWriterLock(int.MaxValue);
                _outputPath.Create();
                using (var sw = File.AppendText(Path.Combine(_outputPath, _logFileName)))
                {
                    var logLine = $"{DateTime.Now:yyyy.MM.dd-HH:mm:ss.ffffff} {logLevel}: {msg}";
                    sw.WriteLine(logLine);
                    sw.Close();
                }
            }
            finally
            {
                Locker.ReleaseWriterLock();
            }
        }

        private void WriteWithException(string message, Exception exception, string logLevel)
        {
            Write($"Message: {message}{Environment.NewLine}Exception: {exception.Message}{Environment.NewLine}" +
                  $"Stack trace: {exception.StackTrace}", logLevel);
        }

        private void WriteWithException(object message, Exception exception, string logLevel)
        {
            Write($"Message: {message}{Environment.NewLine}Exception: {exception.Message}{Environment.NewLine}" +
                  $"Stack trace: {exception.StackTrace}", logLevel);
        }

        public void Info(string message)
        {
            Write(message, InfoLevel);
        }

        public void Info(string message, Exception exception)
        {
            WriteWithException(message, exception, InfoLevel);
        }

        public void Info(object message, Exception exception)
        {
            WriteWithException(message, exception, InfoLevel);
        }

        public void Warn(string message)
        {
            Write(message, WarningLevel);
        }

        public void Warn(string message, Exception exception)
        {
            WriteWithException(message, exception, WarningLevel);
        }

        public void Warn(object message, Exception exception)
        {
            WriteWithException(message, exception, WarningLevel);
        }

        public void Error(string message)
        {
            Write(message, ErrorLevel);
        }

        public void Error(string message, Exception exception)
        {
            WriteWithException(message, exception, ErrorLevel);
        }

        public void Error(object message, Exception exception)
        {
            WriteWithException(message, exception, ErrorLevel);
        }

        public void Debug(string message)
        {
            Write(message, DebugLevel);
        }

        public void Debug(string message, Exception exception)
        {
            WriteWithException(message, exception, DebugLevel);
        }

        public void Debug(object message, Exception exception)
        {
            WriteWithException(message, exception, DebugLevel);
        }

        public void Fatal(string message)
        {
            Write(message, FatalLevel);
        }

        public void Fatal(string message, Exception exception)
        {
            WriteWithException(message, exception, FatalLevel);
        }

        public void Fatal(object message, Exception exception)
        {
            WriteWithException(message, exception, FatalLevel);
        }

        public void Exception(string message)
        {
            Write(message, ExceptionLevel);
        }

        public void Exception(string message, Exception exception)
        {
            WriteWithException(message, exception, ExceptionLevel);
        }

        public void Exception(object message, Exception exception)
        {
            WriteWithException(message, exception, ExceptionLevel);
        }
    }
}
