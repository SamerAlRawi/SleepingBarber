using System;

namespace SleepingBarber.Demo.PersistanceConsole
{
    internal class Customer : ICustomer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
    }
}