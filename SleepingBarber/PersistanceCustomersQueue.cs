using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SleepingBarber
{
    public class PersistanceCustomersQueue<T> : ConcurrentQueue<T>, IPersistanceCustomerQueue<T>
        where T : ICustomer
    {
        private ICustomerRepository<T> _customerRepository;
        public event EventHandler CustomerArrived;

        public PersistanceCustomersQueue(ICustomerRepository<T> customerRepository)
        {
            _customerRepository = customerRepository;
            InitializeQueue();
        }

        private void InitializeQueue()
        {
            var list = _customerRepository.GetAll();

            Task.Run(() => InvokeCustomerArrived(list));

            foreach (var item in list)
            {
                Enqueue(item);
            }
        }

        private void InvokeCustomerArrived(IEnumerable<T> list)
        {
            try
            {
                //wait for subscribers to register, need different pattern or override
                Thread.Sleep(1000);
                if (list.Any() && CustomerArrived != null)
                {
                    CustomerArrived(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                //TODO logging here
            }
        }

        public void Enqueue(T customer)
        {
            _customerRepository.Add(customer);
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
            _customerRepository.Delete(customer);
        }
    }
}