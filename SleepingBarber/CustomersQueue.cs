using System;
using System.Collections.Concurrent;

namespace SleepingBarber
{
    public class CustomersQueue<T> : BlockingCollection<object>, ICustomersQueue<T> where T : ICustomer
    {
        public event EventHandler CustomerArrived;

        public void Enqueue(T customer)
        {
            Add(customer);
            if (CustomerArrived != null)
            {
                CustomerArrived(this, EventArgs.Empty);
            }
        }

        public T Dequeue()
        {
            return (T)Take();
        }
    }
}