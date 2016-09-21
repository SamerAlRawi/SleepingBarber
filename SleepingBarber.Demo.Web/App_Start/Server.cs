using System;
using System.Threading;

namespace SleepingBarber.Demo.Web.App_Start
{
    public class Server<T> : IServer<T> where T : ICustomer
    {
        public void Serve(T customer)
        {
            if (customer.Id != "0")
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Customer {customer.Id} Got his haircut!");
            }
            else
            {
                throw new Exception();
            }
        }
    }
}