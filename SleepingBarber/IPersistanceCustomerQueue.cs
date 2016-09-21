namespace SleepingBarber
{
    public interface IPersistanceCustomerQueue<T> : ICustomersQueue<T> where T : ICustomer
    {

    }
}