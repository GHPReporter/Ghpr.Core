using System;
using Ghpr.Core.Settings;
using Serilog;
using ILogger = Ghpr.Core.Interfaces.ILogger;

namespace Ghpr.SerilogToSeqLogger
{
    public class Logger : ILogger
    {
        private static Serilog.ILogger SerilogLogger => Log.Logger;

        public void SetUp(ProjectSettings reporterSettings)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
            SerilogLogger.Debug("Logger initialization done");
        }

        public void Info(string message)
        {
            SerilogLogger.Information(message);
        }

        public void Info(string message, Exception exception)
        {
            SerilogLogger.Information(message, exception);
        }

        public void Info(object message, Exception exception)
        {
            SerilogLogger.Information(message.ToString(), exception);
        }

        public void Warn(string message)
        {
            SerilogLogger.Warning(message);
        }

        public void Warn(string message, Exception exception)
        {
            SerilogLogger.Warning(message, exception);
        }

        public void Warn(object message, Exception exception)
        {
            SerilogLogger.Warning(message.ToString(), exception);
        }

        public void Error(string message)
        {
            SerilogLogger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            SerilogLogger.Error(message, exception);
        }

        public void Error(object message, Exception exception)
        {
            SerilogLogger.Error(message.ToString(), exception);
        }

        public void Debug(string message)
        {
            SerilogLogger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            SerilogLogger.Debug(message, exception);
        }

        public void Debug(object message, Exception exception)
        {
            SerilogLogger.Debug(message.ToString(), exception);
        }

        public void Fatal(string message)
        {
            SerilogLogger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            SerilogLogger.Fatal(message, exception);
        }

        public void Fatal(object message, Exception exception)
        {
            SerilogLogger.Fatal(message.ToString(), exception);
        }

        public void Exception(string message)
        {
            SerilogLogger.Error(message);
        }

        public void Exception(string message, Exception exception)
        {
            SerilogLogger.Error(message, exception);
        }

        public void Exception(object message, Exception exception)
        {
            SerilogLogger.Error(message.ToString(), exception);
        }

        public void TearDown()
        {
            Log.CloseAndFlush();
        }
    }
}
