using System;

namespace SleepingBarber
{
    public interface ILogger
    {
        void LogException(BarberException exception);
    }

    public class BarberException
    {
        public string ErrorMessage { get; set; }
        public string CustomerType { get; set; }
        public string CustomerId { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return $"Error: {ErrorMessage}\nCustomerType: {CustomerType}\nId: {CustomerId}\nException: {Exception}";
        }
    }
}