using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace SleepingBarber.Persistence.RavenDB
{
    public class RavenDBcustomerRepository<T> : ICustomerRepository<T> where T : ICustomer
    {
        private IDocumentStore _documentStore;
        
        public RavenDBcustomerRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            _documentStore.Initialize();
        }

        public IEnumerable<T> GetAll()
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return GetAllWithLoop<T>(session);
            }
        }

        public void Add(T customer)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Store(customer);
                session.SaveChanges();
            }
        }

        public void Delete(T customer)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                var customerId = customer.Id;

                var instance = session.Load<T>(customerId);
                if (instance != null)
                {
                    session.Delete(instance);
                    session.SaveChanges();
                }
            }
        }

        private static List<T> GetAllWithLoop<T>(IDocumentSession session)
        {
            const int size = 1024;
            int page = 0;

            RavenQueryStatistics stats;
            List<T> objects = session.Query<T>()
                                  .Statistics(out stats)
                                  .Skip(page * size)
                                  .Take(size)
                                  .ToList();

            page++;

            while ((page * size) <= stats.TotalResults)
            {
                objects.AddRange(session.Query<T>()
                             .Skip(page * size)
                             .Take(size)
                             .ToList());
                page++;
            }

            return objects;
        }
    }
}
