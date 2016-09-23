using System;
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
            throw new NotImplementedException();
        }

        public void LogInformation(string logInfo)
        {
            throw new NotImplementedException();
        }
    }
}
