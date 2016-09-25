using Raven.Client;

namespace SleepingBarber.Logging.RavenDB
{
    public class RavenDBLogger : ILogger
    {
        private IDocumentStore _documentStore;

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
