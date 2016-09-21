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

        private static void RegisterTypes(IUnityContainer container)
        {
            //persistance repository
            container.RegisterInstance(typeof(ICustomerRepository<WebCustomer>), new InMemoryRepository<WebCustomer>(),
                new ContainerControlledLifetimeManager());

            var repository = container.Resolve<ICustomerRepository<WebCustomer>>();

            //persistance queue(one instance per domain)
            container.RegisterInstance(typeof(ICustomersQueue<WebCustomer>),
                new PersistanceCustomersQueue<WebCustomer>(repository),
                new ContainerControlledLifetimeManager());

            ////no-persistance queue(one instance per domain)
            //container.RegisterInstance(typeof(ICustomersQueue<WebCustomer>),
            //    new CustomersQueue<WebCustomer>(),
            //    new ContainerControlledLifetimeManager());


            container.RegisterType<IServer<WebCustomer>, Server<WebCustomer>>();
            var server = container.Resolve<IServer<WebCustomer>>();

            var customersQueue = container.Resolve(typeof(ICustomersQueue<WebCustomer>)) as ICustomersQueue<WebCustomer>;
            container.RegisterInstance(typeof(ISleepingBarber<WebCustomer>),
                new SleepingBarber<WebCustomer>(customersQueue, server),
                new ContainerControlledLifetimeManager());

            StaticIOC.Container = container;
        }
    }
}
