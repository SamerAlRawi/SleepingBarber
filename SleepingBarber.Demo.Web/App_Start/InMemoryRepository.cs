using System.Collections.Generic;

namespace SleepingBarber.Demo.Web.App_Start
{
    public class InMemoryRepository<T> : ICustomerRepository<T> where T : ICustomer
    {
        private List<T> _list = new List<T>();
        public IEnumerable<T> GetAll()
        {
            return _list;
        }

        public void Add(T customer)
        {
            _list.Add(customer);
        }

        public void Delete(T customer)
        {
            _list.Remove(customer);
        }
    }
}