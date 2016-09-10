using System;
using Microsoft.Practices.Unity;
using SleepingBarber.Demo.Web.Models;

namespace SleepingBarber.Demo.Web.App_Start
{
    public class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });
        
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance(typeof(ICustomersQueue<WebCustomer>), 
                new CustomersQueue<WebCustomer>(), 
                new ContainerControlledLifetimeManager());

            var customersQueue = container.Resolve(typeof(ICustomersQueue<WebCustomer>)) as ICustomersQueue<WebCustomer>;
            container.RegisterInstance(typeof(ISleepingBarber<WebCustomer>), 
                new SleepingBarber<WebCustomer>(customersQueue), 
                new ContainerControlledLifetimeManager());

            StaticIOC.Container = container;
        }
    }
}
