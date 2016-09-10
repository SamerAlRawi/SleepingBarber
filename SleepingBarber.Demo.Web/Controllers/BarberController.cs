using System.Collections.Generic;
using System.Web.Mvc;
using SleepingBarber.Demo.Web.Models;

namespace SleepingBarber.Demo.Web.Controllers
{
    public class BarberController : Controller
    {
        private ISleepingBarber<WebCustomer> _barber;
        private ICustomersQueue<WebCustomer> _queue;

        public BarberController(ICustomersQueue<WebCustomer> queue, ISleepingBarber<WebCustomer> barber)
        {
            _queue = queue;
            _barber = barber;
        }

        [HttpPost]
        public JsonResult Create(int customers)
        {
            var result = new List<EventResult>();
            for (int i = 0; i < customers; i++)
            {
                if (i%3 == 0)
                {
                    _queue.Enqueue(new ErrorWebCustomer(i));
                    result.Add(new EventResult { Id = i, CustomerName = $"Customer {i}" });
                }
                else
                {
                    _queue.Enqueue(new WebCustomer(i));
                    result.Add(new EventResult {Id = i, CustomerName = $"Customer {i}"});
                }
            }
            return Json(result);
        }
    }
}