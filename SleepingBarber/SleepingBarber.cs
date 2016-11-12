using System;
using System.Threading.Tasks;

namespace SleepingBarber
{
    public class SleepingBarber<T> : ISleepingBarber<T> where T : ICustomer
    {
        private volatile bool _processInProgress;
        private ICustomersQueue<T> _customersQueue;
        private IServer<T> _server;

        /// <summary>
        /// This event invoked when barber finish serving all customers with timestamp of the event time
        /// </summary>
        public event EventHandler<DateTime> GoingToSleep;
        /// <summary>
        /// Listen to this event if you need to track progress or store auditing information
        /// This event invoked when a customer is served, the event argument is the customer Id
        /// </summary>
        public event EventHandler<string> CustomerServed;
        /// <summary>
        /// Invoked if the server fails to serve the given customer
        /// the event argument is the customer instance
        /// </summary>
        public event EventHandler<T> FailedToServiceCustomer;

        public SleepingBarber(ICustomersQueue<T> customersQueue, IServer<T> server)
        {
            _server = server;
            _customersQueue = customersQueue;
            _customersQueue.CustomerArrived += ProcessCustomerQueue;
        }

        private void ProcessCustomerQueue(object sender, EventArgs e)
        {
            if (_processInProgress)
            {
                return;
            }
            _processInProgress = true;
            Task.Run(() =>
            {
                ProcessAll();
            });
        }

        private void ProcessAll()
        {
            while (_customersQueue.Count > 0)
            {
                var customer = _customersQueue.Dequeue();
                try
                {
                    _server.Serve(customer);
                    NotifyCustomerServed(customer);
                    _customersQueue.Delete(customer);
                }
                catch (Exception ex)
                {
                    TryLog(ex, customer);
                    Try(() =>
                    {
                        if (FailedToServiceCustomer != null)
                            FailedToServiceCustomer(this, customer);
                    });
                }
            }
            NotifyGoingToSleep();
            _processInProgress = false;
        }

        private void NotifyGoingToSleep()
        {
            Try(() =>
            {
                if (GoingToSleep != null)
                    GoingToSleep(this, DateTime.Now);
            });
        }

        private void NotifyCustomerServed(ICustomer customer)
        {
            Try(() =>
            {
                if (CustomerServed != null)
                    CustomerServed(this, customer.Id);
            });
        }

        private void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                //log exception here.
            }
        }

        private static void TryLog(Exception ex, T customer)
        {
            if (BarberLogManager.Logger != null)
            {
                BarberLogManager.Logger.LogException(new BarberException
                {
                    ErrorMessage = "Failed to serve customer",
                    CustomerType = typeof(T).FullName,
                    CustomerId = customer.Id,
                    Exception = ex,
                });
            }
        }
    }
}