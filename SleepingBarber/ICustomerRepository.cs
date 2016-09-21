using System.Collections.Generic;

namespace SleepingBarber
{
    public interface ICustomerRepository<T> where T : ICustomer
    {
        IEnumerable<T> GetAll();
        void Add(T customer);
        void Delete(T customer);
    }
}