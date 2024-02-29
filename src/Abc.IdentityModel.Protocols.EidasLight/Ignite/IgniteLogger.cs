#if NETSTANDARD || NETCOREAPP2_0_OR_GREATER

using Apache.Ignite.Core.Log;
using System;

namespace Abc.IdentityModel.EidasLight.Ignite {
    internal class IgniteLogger : ILogger {
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public IgniteLogger(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory) {
            this.logger = loggerFactory.CreateLogger("Apache.Ignite");
        }

        public bool IsEnabled(LogLevel level) {
            return true;
        }

        public void Log(LogLevel level, string message, object[] args, IFormatProvider formatProvider, string category, string nativeErrorInfo, Exception ex) {
            Microsoft.Extensions.Logging.LogLevel eventType = Microsoft.Extensions.Logging.LogLevel.Debug;
            switch (level) {
                case LogLevel.Trace:
                    eventType = Microsoft.Extensions.Logging.LogLevel.Trace;
                    break;
                case LogLevel.Debug:
                    eventType = Microsoft.Extensions.Logging.LogLevel.Debug;
                    break;
                case LogLevel.Info:
                    eventType = Microsoft.Extensions.Logging.LogLevel.Information;
                    break;
                case LogLevel.Warn:
                    eventType = Microsoft.Extensions.Logging.LogLevel.Warning;
                    break;
                case LogLevel.Error:
                    eventType = Microsoft.Extensions.Logging.LogLevel.Error;
                    break;
            }

            this.logger.Log(eventType, 0, message, ex, null); 
        }
    }
}

#endif