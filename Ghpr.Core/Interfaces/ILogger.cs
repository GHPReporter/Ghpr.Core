using System;

namespace Ghpr.Core.Interfaces
{
    public interface ILogger
    {
        void Info(Exception exception);
        void Info(string message, Exception exception);
        void Info(object message, Exception exception);

        void Warn(Exception exception);
        void Warn(string message, Exception exception);
        void Warn(object message, Exception exception);

        void Error(Exception exception);
        void Error(string message, Exception exception);
        void Error(object message, Exception exception);

        void Debug(Exception exception);
        void Debug(string message, Exception exception);
        void Debug(object message, Exception exception);

        void Fatal(Exception exception);
        void Fatal(string message, Exception exception);
        void Fatal(object message, Exception exception);

        void Exception(Exception exception);
        void Exception(string message, Exception exception);
        void Exception(object message, Exception exception);
    }
}