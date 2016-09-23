using System;

namespace SleepingBarber
{
    public interface ILogger
    {
        void LogException(BarberException exception);
        void LogInformation(string logInfo);
    }

    public class BarberException
    {
        public string Category { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}