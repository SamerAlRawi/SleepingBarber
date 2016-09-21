using NUnit.Framework;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class CustomersQueueTests
    {
        private CustomersQueue<CustomerForTest> _customersQueue;

        [SetUp]
        public void Setup()
        {
            _customersQueue = new CustomersQueue<CustomerForTest>();
        }

        [Test]
        public void Enqueue_Dequeue_Add_Customer_To_Queue_And_Retrieves_Accordingly()
        {
            var customer1 = new CustomerForTest();
            var customer2 = new CustomerForTest();
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
                _customersQueue.Enqueue(new CustomerForTest());   
            }
            Assert.That(_customersQueue.Count, Is.EqualTo(customersCount));
        }

        [Test]
        public void Enqueue_Notify_Customer_Arrived()
        {
            bool notified = false;
            _customersQueue.CustomerArrived += (sender, args) =>
            {
                notified = true;
            };
            _customersQueue.Enqueue(new CustomerForTest());
            Assert.IsTrue(notified);
        }
    }
}