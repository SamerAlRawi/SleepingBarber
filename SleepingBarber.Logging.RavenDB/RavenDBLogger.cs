using Raven.Client;

namespace SleepingBarber.Logging.RavenDB
{
    /// <summary>
    /// Logger for SleepingBarber with persistance using RavenDB
    /// </summary>
    public class RavenDBLogger : ILogger
    {
        private IDocumentStore _documentStore;

        /// <summary>
        /// Construct with RavenDB document store address
        /// </summary>
        /// <param name="documentStore">RavenDB docment store address</param>
        public RavenDBLogger(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            _documentStore.Initialize();
        }

        public void LogException(BarberException exception)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Store(exception);
                session.SaveChanges();
            }
        }
    }
}
