using System;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Utils
{
    public class EmptyLogger : ILogger
    {
        public void SetUp(ReporterSettings reporterSettings)
        {
        }

        public void Info(string message)
        {
        }

        public void Info(string message, Exception exception)
        {
        }

        public void Info(object message, Exception exception)
        {
        }

        public void Warn(string message)
        {
        }

        public void Warn(string message, Exception exception)
        {
        }

        public void Warn(object message, Exception exception)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception exception)
        {
        }

        public void Error(object message, Exception exception)
        {
        }

        public void Debug(string message)
        {
        }

        public void Debug(string message, Exception exception)
        {
        }

        public void Debug(object message, Exception exception)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, Exception exception)
        {
        }

        public void Fatal(object message, Exception exception)
        {
        }

        public void Exception(string message)
        {
        }

        public void Exception(string message, Exception exception)
        {
        }

        public void Exception(object message, Exception exception)
        {
        }

        public void TearDown()
        {
        }
    }
}