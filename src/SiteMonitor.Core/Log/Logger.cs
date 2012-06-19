using System;
using log4net;
using log4net.Config;

namespace SiteMonitor.Core.Log
{
    internal class Logger
    {
        private static Logger _instance;
        private static object _syncRoot = new object();

        private ILog _logger;

        private Logger()
        {
            _logger = LogManager.GetLogger("SiteMonitor");
        }

        internal void Log(LogLevel level, string message, params object[] parameters)
        {
            Log(level, null, message, parameters);
        }

        internal void Log(LogLevel level, Exception ex, string message, params object[] parameters)
        {
            if (
                (level == LogLevel.Debug && _logger.IsDebugEnabled) ||
                (level == LogLevel.Error && _logger.IsErrorEnabled) ||
                (level == LogLevel.Information && _logger.IsInfoEnabled) ||
                (level == LogLevel.Fatal && _logger.IsFatalEnabled))
            {
                message = String.Format(message.Replace("{", "<{").Replace("}", "}>"), parameters);

                if (level == LogLevel.Debug)
                {
                    _logger.Debug(message);
                }
                else if (level == LogLevel.Information)
                {
                    _logger.Info(message);
                }
                else if (level == LogLevel.Fatal)
                {
                    if (ex != null)
                        _logger.Fatal(message, ex);
                    else
                        _logger.Error(message);
                }
                else if (level == LogLevel.Error)
                {
                    if (ex != null)
                    {
                        LogInnerException(message, ex, LogLevel.Error);
                    }
                    else
                        _logger.Error(message);
                }
            }
        }

        private void LogInnerException(string message, Exception ex, LogLevel level)
        {
            if (level == LogLevel.Error)
                _logger.Error(message, ex);
            else if (level == LogLevel.Fatal)
                _logger.Fatal(message, ex);

            if (ex.InnerException != null)
            {
                LogInnerException("Inner Exception", ex.InnerException, level);
            }
        }

        internal static Logger GetLogger()
        {
            XmlConfigurator.Configure();

            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    _instance = new Logger();
                }
            }

            return _instance;
        }
    }

    public enum LogLevel
    {
        Information,
        Debug,
        Error,
        Fatal
    }
}
