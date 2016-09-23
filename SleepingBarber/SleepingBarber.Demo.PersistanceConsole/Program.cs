using System;
using System.Threading;
using Raven.Client.Document;
using SleepingBarber.Persistance.RavenDB;

namespace SleepingBarber.Demo.PersistanceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Can use any RavenDb implementation
             * EmbeddableDocumentStore, 
             * RavenDbDocumentStore,
             * DocumentStore
             */
            var ravenDbdocumentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Customers",
                //ConnectionStringName = "ConnectionFromWebConfig"
            };
            var repository = new RavenDBcustomerRepository<Customer>(ravenDbdocumentStore);
            var queue = new PersistanceCustomersQueue<Customer>(repository);
            var server = new Server<Customer>();
            var barber = new SleepingBarber<Customer>(queue, server);
            barber.CustomerServed += CustomerServed;
            barber.GoingToSleep += BarberWentToSleep;
            for (int i = 0; i <50; i++)
            {
                var name = $"Customer{i}";
                queue.Enqueue(new Customer { Id = name, Name = name, DateCreate = DateTime.Now });
                Console.WriteLine($"{i} was added to database");
            }
            Console.ReadLine();
        }

        private static void BarberWentToSleep(object sender, DateTime e)
        {
            Console.WriteLine("Barber went to sleep Zzzz, \ncustomers queue is empty.");
        }

        private static void CustomerServed(object sender, string customerId)
        {
            Console.WriteLine($"Barber says Customer {customerId} is served.");
        }
    }

    public class Server<T> : IServer<T> where T : ICustomer
    {
        public void Serve(T customer)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"Customer {customer.Id} Got his haircut!");
        }
    }
}
