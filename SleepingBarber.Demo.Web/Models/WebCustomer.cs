using System;
using System.Threading;

namespace SleepingBarber.Demo.Web.Models
{
    public class WebCustomer : ICustomer
    {
        public string Id { get; set; }
        public virtual void Serve()
        {
            var r = new Random(1000);
            Thread.Sleep(r.Next(1750, 9000));
        }
    }
}