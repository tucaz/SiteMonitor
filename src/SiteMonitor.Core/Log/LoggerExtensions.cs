using System;

namespace SiteMonitor.Core.Log
{
    public static class LoggerExtensions
    {
        public static void Log(this string message, LogLevel level, params object[] parameters)
        {
            var logger = Logger.GetLogger();
            logger.Log(level, message, parameters);
        }

        public static void LogDebug(this string message, params object[] parameters)
        {
            var logger = Logger.GetLogger();
            logger.Log(LogLevel.Debug, message, parameters);
        }

        public static void LogInformation(this string message, params object[] parameters)
        {
            var logger = Logger.GetLogger();
            logger.Log(LogLevel.Information, message, parameters);
        }

        public static void LogError(this Exception error, string mensagem, params object[] parameters)
        {
            var logger = Logger.GetLogger();
            logger.Log(LogLevel.Error, error, mensagem, parameters);
        }

        public static void LogError(this Exception error)
        {
            LogError(error, String.Empty);
        }

        public static void LogFatal(this Exception error)
        {
            var logger = Logger.GetLogger();
            error.Message.Log(LogLevel.Fatal);
        }
    }
}
