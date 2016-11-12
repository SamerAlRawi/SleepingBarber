using System.Threading;
using System;
using static System.Console;

namespace SleepingBarber.Demo.Console
{
    class Program
    {
        private static CustomersQueue<Customer> _queue;
        private static SleepingBarber<Customer> _sleepingBarber;
        private static IServer<Customer> _server;

        static void Main(string[] args)
        {
            bool exitRequested = false;
            _queue = new CustomersQueue<Customer>();
            _server = new CustomerServer();
            _sleepingBarber = new SleepingBarber<Customer>(_queue, _server);
            _sleepingBarber.GoingToSleep += BarberWentToSleep;
            WriteLine("Enter customers range to add then press ENTER");
            WriteLine("Enter Q to Exit");
            
            while (!exitRequested)
            {
                var input = ReadLine();
                exitRequested = input.ToUpper() == "Q";
                AddCustomers(input);
            }
        }

        private static void BarberWentToSleep(object sender, DateTime e)
        {
            WriteLine($"Barber went to sleep @ {e}");
        }

        private static void AddCustomers(string input)
        {
            try
            {
                var count = int.Parse(input, 0);
                for (int i = 0; i < count; i++)
                {
                    _queue.Enqueue(new Customer(i+1));
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                WriteLine("Press Q to Exit....");
            }
        }
    }

    public class Customer : ICustomer
    {
        private int _id;

        public Customer(int id)
        {
            _id = id;
        }

        public string Id
        {
            get { return _id.ToString(); }
            set { }
        }
    }

    public class CustomerServer : IServer<Customer>
    {
        public void Serve(Customer customer)
        {
            Thread.Sleep(1000);
            WriteLine($"Customer {customer.Id} Got his haircut!");
        }
    }
}
