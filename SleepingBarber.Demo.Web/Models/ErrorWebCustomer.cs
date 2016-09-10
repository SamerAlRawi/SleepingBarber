using System;
using System.Threading;

namespace SleepingBarber.Demo.Web.Models
{
    public class ErrorWebCustomer : WebCustomer
    {
        private int _id;

        public ErrorWebCustomer(int id) : base(id)
        {
            _id = id;
        }

        public string Id => _id.ToString();

        public override void Serve()
        {
            var r = new Random(1000);
            Thread.Sleep(r.Next(1750, 9000));
            throw new Exception("simulating execution error");
        }
    }
}