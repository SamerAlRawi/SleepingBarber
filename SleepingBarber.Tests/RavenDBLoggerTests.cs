using NSubstitute;
using NUnit.Framework;
using Raven.Client;
using SleepingBarber.Logging.RavenDB;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class RavenDBLoggerTests
    {
        private RavenDBLogger _repository;
        private IDocumentStore _documentStore;
        private IDocumentSession _defaultSession;

        [SetUp]
        public void Setup()
        {
            _defaultSession = Substitute.For<IDocumentSession>();
            _documentStore = Substitute.For<IDocumentStore>();
            _documentStore.OpenSession().Returns(_defaultSession);
            _repository = new RavenDBLogger(_documentStore);
        }

        [Test]
        public void Initialize_Store()
        {
            _documentStore.Received().Initialize();
        }

        [Test]
        public void Add_Adds_Customer_To_Store()
        {
            var exception = new BarberException();
            _repository.LogException(exception);

            Received.InOrder(() =>
            {
                _defaultSession.Store(exception);
                _defaultSession.SaveChanges();
                _defaultSession.Dispose();
            });
        }
        
    }
}