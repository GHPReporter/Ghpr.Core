using System;
using System.Collections.Generic;
using System.Linq;

namespace Ghpr.SimpleFileLogger.Core
{
    public static class LogLevelExtensions
    {
        public static string GetPrefix(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.None:
                    return "NONE";
                case LogLevel.Fatal:
                    return "FATAL";
                case LogLevel.Exception:
                    return "EXCEPTION";
                case LogLevel.Error:
                    return "ERROR";
                case LogLevel.Warning:
                    return "WARN";
                case LogLevel.Info:
                    return "INFO";
                case LogLevel.Debug:
                    return "DEBUG";
                case LogLevel.All:
                    return "ALL";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Can't find correct prefix");
            }
        }

        private static IEnumerable<LogLevel> GetLevelsToBeLogged(this LogLevel loggerLogLevel)
        {
            switch (loggerLogLevel)
            {
                case LogLevel.None:
                    return new LogLevel[] { };
                case LogLevel.Fatal:
                    return new[] { LogLevel.Fatal };
                case LogLevel.Exception:
                    return new[] { LogLevel.Fatal, LogLevel.Exception };
                case LogLevel.Error:
                    return new[] { LogLevel.Fatal, LogLevel.Exception, LogLevel.Error };
                case LogLevel.Warning:
                    return new[] { LogLevel.Fatal, LogLevel.Exception, LogLevel.Error, LogLevel.Warning };
                case LogLevel.Info:
                    return new[] { LogLevel.Fatal, LogLevel.Exception, LogLevel.Error, LogLevel.Warning, LogLevel.Info };
                case LogLevel.Debug:
                    return new[] { LogLevel.Fatal, LogLevel.Exception, LogLevel.Error, LogLevel.Warning, LogLevel.Info, LogLevel.Debug };
                case LogLevel.All:
                    return new[] { LogLevel.Fatal, LogLevel.Exception, LogLevel.Error, LogLevel.Warning, LogLevel.Info, LogLevel.Debug };
                default:
                    throw new ArgumentOutOfRangeException(nameof(loggerLogLevel), loggerLogLevel, null);
            }
        }

        public static bool SkipMessage(this LogLevel messageLogLevel, LogLevel loggerLogLevel)
        {
            var levels = loggerLogLevel.GetLevelsToBeLogged();
            return !levels.Contains(messageLogLevel);
        }
    }
}