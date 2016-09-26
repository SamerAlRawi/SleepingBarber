using System;

namespace SleepingBarber
{
    public static class BarberLogManager
    {
        private static ILogger _instance;
        public static ILogger Logger
        {
            get { return _instance; }
            set { _instance = new LoggerWrapper(value);}
        }
    }

    internal class LoggerWrapper : ILogger
    {
        private ILogger _instance;

        public LoggerWrapper(ILogger instance)
        {
            _instance = instance;
        }

        public void LogException(BarberException exception)
        {
            try
            {
                _instance.LogException(exception);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
