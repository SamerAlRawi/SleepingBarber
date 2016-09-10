using System;

namespace SleepingBarber
{
    public interface ICustomersQueue<T> where T : ICustomer
    {
        event EventHandler CustomerArrived;
        void Enqueue(T customer);
        T Dequeue();
        int Count { get; }
    }
}
