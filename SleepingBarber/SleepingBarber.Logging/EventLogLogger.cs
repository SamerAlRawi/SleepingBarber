using System.Diagnostics;

namespace SleepingBarber.Logging
{
    public class EventLogLogger : ILogger
    {
        private string _source = "SleepingBarber";
        private string _logName = "Application";

        public EventLogLogger()
        {
            if (!EventLog.SourceExists(_source))
                EventLog.CreateEventSource(_source, _logName);
        }

        public void LogException(BarberException exception)
        {
            EventLog.WriteEntry(_source, exception.ToString());
        }
    }
}