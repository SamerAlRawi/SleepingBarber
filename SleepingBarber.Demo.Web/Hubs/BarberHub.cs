using Microsoft.AspNet.SignalR;
using SleepingBarber.Demo.Web.Models;

namespace SleepingBarber.Demo.Web.Hubs
{
    public class BarberHub : Hub
    {
        private ISleepingBarber<WebCustomer> _sleepingBarber;

        public BarberHub(ISleepingBarber<WebCustomer> sleepingBarber)
        {
            _sleepingBarber = sleepingBarber;
            _sleepingBarber.CustomerServed += (sender, s) => CustomerServed(s);
            _sleepingBarber.FailedToServiceCustomer += (sender, s) => FailedToServe(s);
        }

        private void FailedToServe(WebCustomer customer)
        {
            Clients.All.failedToServe(customer.Id);
        }

        private void CustomerServed(string id)
        {
            Clients.All.customerServed(id);
        }
    }
}