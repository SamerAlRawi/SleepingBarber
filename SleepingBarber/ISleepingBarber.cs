using System;

namespace SleepingBarber
{
    public interface ISleepingBarber<T> where T : ICustomer
    {
        event EventHandler<DateTime> GoingToSleep;
        event EventHandler<string> CustomerServed;
        event EventHandler<T> FailedToServiceCustomer;
    }
}