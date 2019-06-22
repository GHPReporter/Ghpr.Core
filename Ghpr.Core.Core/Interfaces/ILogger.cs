using System;
using Ghpr.Core.Core.Settings;

namespace Ghpr.Core.Core.Interfaces
{
    public interface ILogger
    {
        void SetUp(ProjectSettings reporterSettings);

        void Info(string message);
        void Info(string message, Exception exception);
        void Info(object message, Exception exception);

        void Warn(string message);
        void Warn(string message, Exception exception);
        void Warn(object message, Exception exception);

        void Error(string message);
        void Error(string message, Exception exception);
        void Error(object message, Exception exception);

        void Debug(string message);
        void Debug(string message, Exception exception);
        void Debug(object message, Exception exception);

        void Fatal(string message);
        void Fatal(string message, Exception exception);
        void Fatal(object message, Exception exception);

        void Exception(string message);
        void Exception(string message, Exception exception);
        void Exception(object message, Exception exception);

        void TearDown();
    }
}