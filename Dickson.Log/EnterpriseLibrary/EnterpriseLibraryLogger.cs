using Microsoft.Extensions.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System;
using System.Diagnostics;

namespace Dickson.Logging.EnterpriseLibrary
{
    public class EnterpriseLibraryLogger : ILogger, IDisposable
    {
        LogWriter m_LogWriter;

        public EnterpriseLibraryLogger(LoggingConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            m_LogWriter = new LogWriter(config);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Dispose()
        {
            m_LogWriter.Dispose();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return m_LogWriter.ShouldLog(new LogEntry() { Severity = ToTraceEventType(logLevel) });
        }

        static TraceEventType ToTraceEventType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return TraceEventType.Verbose;
                case LogLevel.Information:
                    return TraceEventType.Information;
                case LogLevel.Warning:
                    return TraceEventType.Warning;
                case LogLevel.Error:
                    return TraceEventType.Error;
                case LogLevel.Critical:
                    return TraceEventType.Critical;
                default:
                    throw new ArgumentOutOfRangeException("level");
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var eventType = ToTraceEventType(logLevel);
            var message = string.Empty;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                message = new TextFormatter().Format(new LogEntry() { Severity = ToTraceEventType(logLevel) });
            }
            if (!string.IsNullOrEmpty(message))
            {
                m_LogWriter.Write(message, logLevel.ToString(), 0, eventId.Id, eventType);
            }
        }
    }
}