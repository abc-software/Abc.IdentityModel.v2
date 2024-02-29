#if NETFRAMEWORK

namespace Abc.IdentityModel.EidasLight.Ignite {
    using Apache.Ignite.Core.Log;
    using System;
    using System.Diagnostics;

    internal class TraceSourceLogger : ILogger {
        private TraceSource traceSource = new TraceSource("Apache.Ignite");

        public bool IsEnabled(LogLevel level) {
            return true;
        }

        public void Log(LogLevel level, string message, object[] args, IFormatProvider formatProvider, string category, string nativeErrorInfo, Exception ex) {
            TraceEventType eventType = TraceEventType.Verbose;
            switch (level) {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    eventType = TraceEventType.Verbose;
                    break;
                case LogLevel.Info:
                    eventType = TraceEventType.Information;
                    break;
                case LogLevel.Warn:
                    eventType = TraceEventType.Warning;
                    break;
                case LogLevel.Error:
                    eventType = TraceEventType.Error;
                    break;
            }

            if (args != null) {
                traceSource.TraceEvent(eventType, -1, message, args);
            }
            else {
                traceSource.TraceEvent(eventType, -1, message);
            }
        }
    }
}

#endif