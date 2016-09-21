using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SleepingBarber
{
    public class PersistanceCustomersQueue<T> : BlockingCollection<object>, IPersistanceCustomerQueue<T>
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
                Add(item);
            }
        }

        private void InvokeCustomerArrived(IEnumerable<T> list)
        {
            try
            {
                //wait for subscribers to register, need different mechanism or override
                Thread.Sleep(1000);
                if (list.Any() && CustomerArrived != null)
                {
                    CustomerArrived(this, EventArgs.Empty);
                }
            }
            catch (Exception)
            {
                //TODO logging here
            }
        }

        public void Enqueue(T customer)
        {
            _customerRepository.Add(customer);
            Add(customer);
            if (CustomerArrived != null)
            {
                CustomerArrived(this, EventArgs.Empty);
            }
        }

        public T Dequeue()
        {
            var dequeue = (T)Take();
            _customerRepository.Delete(dequeue);
            return dequeue;
        }
    }
}