using System.Threading;
using NSubstitute;
using NUnit.Framework;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class CustomersQueueTests
    {
        private CustomersQueue<Customer> _customersQueue;

        [SetUp]
        public void Setup()
        {
            _customersQueue = new CustomersQueue<Customer>();
        }

        [Test]
        public void EnqueueDequeue_Add_Customer_To_Queue_And_Retrieves_Accordingly()
        {
            var customer1 = new Customer();
            var customer2 = new Customer();
            _customersQueue.Enqueue(customer1);
            _customersQueue.Enqueue(customer2);

            Assert.That(_customersQueue.Dequeue(), Is.EqualTo(customer1));
            Assert.That(_customersQueue.Dequeue(), Is.EqualTo(customer2));
        }
        
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(8)]
        [TestCase(20)]
        public void Count_Returns_Expected_Number_Of_Customers(int customersCount)
        {
            for (int c = 0; c < customersCount; c++)
            {
                _customersQueue.Enqueue(new Customer());   
            }
            Assert.That(_customersQueue.Count, Is.EqualTo(customersCount));
        }
    }

    internal class Customer : ICustomer
    {
        public bool Served { get; private set; }
        public string Id { get; }

        public void Serve()
        {
            Served = true;
        }
    }
}