using System;
using System.IO;
using System.Threading;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;

namespace Ghpr.SimpleFileLogger
{
    public class Logger : ILogger
    {
        private string _outputPath;
        private string _fileName;
        private LogLevel _loggerLogLevel;
        private static readonly ReaderWriterLock Locker = new ReaderWriterLock();
        
        public void SetUp(ReporterSettings reporterSettings)
        {
            var settings = "Ghpr.SimpleFileLogger.Settings.json".LoadSettingsAs<LoggerSettings>();
            _outputPath = settings.OutputPath ?? reporterSettings.OutputPath;
            _fileName = settings.FileName ?? "GhprLog.txt";
            var success = Enum.TryParse(settings.LogLevel, out _loggerLogLevel);
            if (!success)
            {
                _loggerLogLevel = LogLevel.Info;
            }
        }
        
        private void Write(string msg, LogLevel messageLogLevel)
        {
            if (messageLogLevel.SkipMessage(_loggerLogLevel))
            {
                return;
            }
            try
            {
                Locker.AcquireWriterLock(int.MaxValue);
                _outputPath.Create();
                using (var sw = File.AppendText(Path.Combine(_outputPath, _fileName)))
                {
                    var logLine = $"{DateTime.Now:yyyy.MM.dd-HH:mm:ss.ffffff} {messageLogLevel.GetPrefix()}: {msg}";
                    sw.WriteLine(logLine);
                    sw.Close();
                }
            }
            finally
            {
                Locker.ReleaseWriterLock();
            }
        }

        private void WriteWithException(string message, Exception exception, LogLevel logLevel)
        {
            Write($"Message: {message}{Environment.NewLine}Exception: {exception.Message}{Environment.NewLine}" +
                  $"Stack trace: {exception.StackTrace}", logLevel);
        }

        private void WriteWithException(object message, Exception exception, LogLevel logLevel)
        {
            Write($"Message: {message}{Environment.NewLine}Exception: {exception.Message}{Environment.NewLine}" +
                  $"Stack trace: {exception.StackTrace}", logLevel);
        }

        public void Info(string message)
        {
            Write(message, LogLevel.Info);
        }

        public void Info(string message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Info);
        }

        public void Info(object message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Info);
        }

        public void Warn(string message)
        {
            Write(message, LogLevel.Warning);
        }

        public void Warn(string message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Warning);
        }

        public void Warn(object message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Warning);
        }

        public void Error(string message)
        {
            Write(message, LogLevel.Error);
        }

        public void Error(string message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Error);
        }

        public void Error(object message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Error);
        }

        public void Debug(string message)
        {
            Write(message, LogLevel.Debug);
        }

        public void Debug(string message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Debug);
        }

        public void Debug(object message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Debug);
        }

        public void Fatal(string message)
        {
            Write(message, LogLevel.Fatal);
        }

        public void Fatal(string message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Fatal);
        }

        public void Fatal(object message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Fatal);
        }

        public void Exception(string message)
        {
            Write(message, LogLevel.Exception);
        }

        public void Exception(string message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Exception);
        }

        public void Exception(object message, Exception exception)
        {
            WriteWithException(message, exception, LogLevel.Exception);
        }

        public void TearDown()
        {
            
        }
    }
}
