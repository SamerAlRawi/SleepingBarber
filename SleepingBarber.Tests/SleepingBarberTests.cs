﻿using System;
using System.Threading;
using NSubstitute;
using NUnit.Framework;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class SleepingBarberTests
    {
        private SleepingBarber<ICustomer> _sleepingBarber;
        private ICustomersQueue<ICustomer> _customersQueue;

        [SetUp]
        public void Setup()
        {
            _customersQueue = Substitute.For<ICustomersQueue<ICustomer>>();
            _sleepingBarber = new SleepingBarber<ICustomer>(_customersQueue);
        }

        [Test]
        public void Barber_Process_CustomersQueue_On_CustomerArrived_Event()
        {
            var customer1Mock = Substitute.For<ICustomer>();
            var customer2Mock = Substitute.For<ICustomer>();
            _customersQueue.Dequeue().Returns(customer1Mock, customer2Mock);
            _customersQueue.Count.Returns(2, 1, 0);
            _customersQueue.CustomerArrived += Raise.Event();

            Thread.Sleep(1000);

            Received.InOrder(() =>
            {
                customer1Mock.Serve();
                customer2Mock.Serve();
            });
        }

        [Test]
        public void Barber_Goes_To_Sleep_After_Serving_All_Customers()
        {
            bool barberWentToSleep = false;
            object eventSender = null;
            DateTime sleptAt = DateTime.Now;
            _sleepingBarber.GoingToSleep += (sender, time) =>
            {
                barberWentToSleep = true;
                sleptAt = time;
                eventSender = sender;
            };

            var customer1Mock = Substitute.For<ICustomer>();
            var customer2Mock = Substitute.For<ICustomer>();
            _customersQueue.Dequeue().Returns(customer1Mock, customer2Mock);
            _customersQueue.Count.Returns(2, 1, 0);
            _customersQueue.CustomerArrived += Raise.Event();


            Thread.Sleep(1000);

            Assert.That(barberWentToSleep, Is.EqualTo(true), "Oops, the barber didn't go to sleep.");
            Assert.That(eventSender, Is.EqualTo(_sleepingBarber));
            Assert.IsTrue(sleptAt < DateTime.Now && sleptAt > DateTime.Now.Subtract(TimeSpan.FromSeconds(2)));
        }

        [Test]
        public void Barber_Notify_Listeners_When_A_Customer_Is_Served()
        {
            string expected = "Customer X";

            object eventSender = null;
            string actual = string.Empty;
            _sleepingBarber.CustomerServed += (sender, id) =>
            {
                eventSender = sender;
                actual = id;
            };

            var customer1Mock = Substitute.For<ICustomer>();
            customer1Mock.Id.Returns(expected);
            _customersQueue.Dequeue().Returns(customer1Mock);
            _customersQueue.Count.Returns(1, 0);
            _customersQueue.CustomerArrived += Raise.Event();
            
            Thread.Sleep(1000);

            Assert.That(actual, Is.EqualTo(expected), "Oops, the barber didn't go to sleep.");
            Assert.That(eventSender, Is.EqualTo(_sleepingBarber));
        }

        [Test]
        public void Barber_Notify_Listeners_When_CustomerService_Failed()
        {
            string expected = "CustomerX";

            object eventSender = null;
            string actual = string.Empty;
            _sleepingBarber.FailedToServiceCustomer += (sender, id) =>
            {
                eventSender = sender;
                actual = id;
            };

            var customer1Mock = Substitute.For<ICustomer>();
            customer1Mock.When(c => c.Serve()).Do((_) =>
            {
                throw new Exception();
            });
            customer1Mock.Id.Returns(expected);
            _customersQueue.Dequeue().Returns(customer1Mock);
            _customersQueue.Count.Returns(1, 0);
            _customersQueue.CustomerArrived += Raise.Event();
            
            Thread.Sleep(1000);

            Assert.That(actual, Is.EqualTo(expected), "Oops, the barber didn't go to sleep.");
            Assert.That(eventSender, Is.EqualTo(_sleepingBarber));
        }
        
    }
}