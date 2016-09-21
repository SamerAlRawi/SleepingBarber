using System;

namespace SleepingBarber
{
    public interface IServer<T> where T : ICustomer
    {
        void Serve(T customer);
    }
}