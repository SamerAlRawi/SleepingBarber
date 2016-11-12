using System;
using System.Diagnostics;

namespace SleepingBarber.Logging
{
    /// <summary>
    /// SleepingBarber event log logger
    /// </summary>
    public class EventLogLogger : ILogger
    {
        private string _source;
        private string _logName = "Application";
        /// <summary>
        /// construct with a valid eventLogsource, or a new eventLogSource to be added(require admin permissions)
        /// </summary>
        /// <param name="eventLogSource"></param>
        public EventLogLogger(string eventLogSource = "SleepingBarber")
        {
            if(string.IsNullOrEmpty(eventLogSource))
                throw new ArgumentNullException(nameof(eventLogSource));

            _source = eventLogSource;

            if (!EventLog.SourceExists(_source))
                EventLog.CreateEventSource(_source, _logName);
        }
        /// <summary>
        /// Exception handling
        /// </summary>
        /// <param name="exception"></param>
        public void LogException(BarberException exception)
        {
            EventLog.WriteEntry(_source, exception.ToString());
        }
    }
}