using System;
using System.Collections.Concurrent;

namespace SleepingBarber
{
    public class CustomersQueue<T> : ConcurrentQueue<T>, ICustomersQueue<T> where T : ICustomer
    {
        public event EventHandler CustomerArrived;

        public void Enqueue(T customer)
        {
            base.Enqueue(customer);
            if (CustomerArrived != null)
            {
                CustomerArrived(this, EventArgs.Empty);
            }
        }

        public T Dequeue()
        {
            T result = default(T);
            TryDequeue(out result);
            return result;
        }

        public void Delete(T customer)
        {
            //no operation needed, for persistance queues only
        }
    }
}