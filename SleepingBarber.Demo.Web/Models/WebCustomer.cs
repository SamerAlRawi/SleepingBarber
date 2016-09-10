using System;
using System.Threading;

namespace SleepingBarber.Demo.Web.Models
{
    public class WebCustomer : ICustomer
    {
        private int _id;

        public WebCustomer(int id)
        {
            _id = id;
        }

        public string Id => _id.ToString();

        public virtual void Serve()
        {
            var r = new Random(1000);
            Thread.Sleep(r.Next(1750, 9000));
        }
    }
}