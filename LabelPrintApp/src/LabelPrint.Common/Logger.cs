using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace LabelPrint.Common
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static Logger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var xx = XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            //log4net.Config.XmlConfigurator.Configure();
        }
        public static bool IsLoggerEnabled = true;

        public static void Debug(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Debug((object)message);
            }
        }

        public static void Debug(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Debug((object)message, exception);
            }
        }

        public static void DebugFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.DebugFormat(format, args);
            }
        }

        public static void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.DebugFormat(formatProvider, format, args);
            }
        }

        public static void Info(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Info((object)message);
            }
        }

        public static void Info(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Info((object)message, exception);
            }
        }

        public static void InfoFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.InfoFormat(format, args);
            }
        }

        public static void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.InfoFormat(formatProvider, format, args);
            }
        }

        public static void Warn(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Warn((object)message);
            }
        }

        public static void Warn(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Warn((object)message, exception);
            }
        }

        public static void WarnFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(format, args);
            }
        }

        public static void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(formatProvider, format, args);
            }
        }

        public static void Error(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Error((object)message);
            }
        }

        public static void Error(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Error((object)message, exception);
            }
        }

        public static void ErrorFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(format, args);
            }
        }

        public static void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(formatProvider, format, args);
            }
        }

        public static void Fatal(this string message)
        {
            if (log != null)
            {
                log.Fatal((object)message);
            }
        }

        public static void Fatal(this string message, Exception exception)
        {
            if (log != null)
            {
                log.Fatal((object)message, exception);
            }
        }

        public static void FatalFormat(this string format, params object[] args)
        {
            if (log != null)
            {
                log.FatalFormat(format, args);
            }
        }

        public static void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (log != null)
            {
                log.FatalFormat(formatProvider, format, args);
            }
        }
    }
}
