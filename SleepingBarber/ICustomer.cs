namespace SleepingBarber
{
    public interface ICustomer
    {
        string Id { get; }
        void Serve();
    }
}