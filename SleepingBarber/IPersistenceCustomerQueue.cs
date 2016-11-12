namespace SleepingBarber
{
    public interface IPersistenceCustomerQueue<T> : ICustomersQueue<T> where T : ICustomer
    {

    }
}