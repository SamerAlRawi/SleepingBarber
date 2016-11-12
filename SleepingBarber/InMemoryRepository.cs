using System.Collections.Generic;

namespace SleepingBarber
{
    public class InMemoryRepository<T> : ICustomerRepository<T> where T : ICustomer
    {
        private List<T> _list = new List<T>();
        object _lock = new object();

        public IEnumerable<T> GetAll()
        {
            return _list;
        }

        public void Add(T customer)
        {
            lock (_lock)
            {
                _list.Add(customer);
            }
        }

        public void Delete(T customer)
        {
            lock (_lock)
            {
                _list.Remove(customer);
            }
        }
    }
}