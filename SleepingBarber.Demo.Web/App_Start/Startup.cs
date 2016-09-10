using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;
using SleepingBarber.Demo.Web.App_Start;
using SleepingBarber.Demo.Web.Hubs;
using SleepingBarber.Demo.Web.Models;

[assembly: OwinStartup(typeof(Startup))]
namespace SleepingBarber.Demo.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ISleepingBarber<WebCustomer> barber = StaticIOC.Container.Resolve<ISleepingBarber<WebCustomer>>();
            var barberHub = new BarberHub(barber);

            GlobalHost.DependencyResolver.Register(typeof(BarberHub), () => barberHub);

            app.MapSignalR();
        }
    }
}