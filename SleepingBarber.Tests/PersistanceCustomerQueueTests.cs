using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NSubstitute;
using NUnit.Framework;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class PersistanceCustomerQueueTests
    {
        private PersistanceCustomersQueue<CustomerForTest> _customersQueue;
        private ICustomerRepository<CustomerForTest> _repository;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<ICustomerRepository<CustomerForTest>>();
            _customersQueue = new PersistanceCustomersQueue<CustomerForTest>(_repository);
        }

        [Test]
        public void Initialize_Queue_From_Repository()
        {
            var defaultList = new List<CustomerForTest> { new CustomerForTest(), new CustomerForTest() };

            _repository.GetAll().Returns(defaultList);
            _customersQueue = new PersistanceCustomersQueue<CustomerForTest>(_repository);

            var first = _customersQueue.Dequeue();
            var second = _customersQueue.Dequeue();

            Assert.That(first, Is.EqualTo(defaultList[0]));
            Assert.That(second, Is.EqualTo(defaultList[1]));
        }

        [Test]
        public void Initialize_Triggers_CustomerArrived_If_Database_Contains_Any_Customer()
        {
            bool customerArrived = false;
            var defaultList = ParallelEnumerable.Repeat(new CustomerForTest(), 1000);
            _repository.GetAll().Returns(defaultList);

            _customersQueue = new PersistanceCustomersQueue<CustomerForTest>(_repository);
            _customersQueue.CustomerArrived += (sender, args) => customerArrived = true;
            var starTime = DateTime.Now;

            //wait until the event triggered or timeout.
            while (!customerArrived && starTime > DateTime.Now.Subtract(TimeSpan.FromSeconds(5)))
            {
                Thread.Sleep(100);
            }

            Assert.IsTrue(customerArrived);
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

        [Test]
        public void Enqueue_Adds_Customer_to_Repository()
        {
            var customer1 = new CustomerForTest();
            _customersQueue.Enqueue(customer1);

            _repository.Received().Add(customer1);
        }

        [Test]
        public void Dequeue_Delete_Customer_Using_Repository()
        {
            var customer1 = new CustomerForTest();
            _customersQueue.Enqueue(customer1);
            _customersQueue.Dequeue();
            _repository.Received().Add(customer1);
        }

        [Test]
        public void Delete_Delete_Customer_Using_Repository()
        {
            var customer1 = new CustomerForTest();
            _customersQueue.Enqueue(customer1);
            var customer = _customersQueue.Dequeue();
            _customersQueue.Delete(customer);

            _repository.Received().Delete(customer1);
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