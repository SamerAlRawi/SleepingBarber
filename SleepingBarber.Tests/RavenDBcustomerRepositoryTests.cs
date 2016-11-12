using NSubstitute;
using NUnit.Framework;
using Raven.Client;
using SleepingBarber.Persistence.RavenDB;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class RavenDBcustomerRepositoryTests
    {
        private RavenDBcustomerRepository<CustomerFake> _repository;
        private IDocumentStore _documentStore;
        private IDocumentSession _defaultSession;

        [SetUp]
        public void Setup()
        {
            _defaultSession = Substitute.For<IDocumentSession>();
            _documentStore = Substitute.For<IDocumentStore>();
            _documentStore.OpenSession().Returns(_defaultSession);
            _repository = new RavenDBcustomerRepository<CustomerFake>(_documentStore);
        }

        [Test]
        public void Initialize_Store()
        {
            _documentStore.Received().Initialize();
        }

        [Test]
        public void Add_Adds_Customer_To_Store()
        {
            var customerFake = new CustomerFake();
            _repository.Add(customerFake);

            Received.InOrder(() =>
            {
                _defaultSession.Store(customerFake);
                _defaultSession.SaveChanges();
                _defaultSession.Dispose();
            });
        }

        [Test]
        public void Delete_Retrieves_Customer_And_Delete_From_Store()
        {
            string id= "1001";
            var customerFake = new CustomerFake {Id = id};
            var instance = new CustomerFake();

            _defaultSession.Load<CustomerFake>(id).Returns(instance);

           _repository.Delete(customerFake);

            Received.InOrder(() =>
            {
                _defaultSession.Delete(instance);
                _defaultSession.SaveChanges();
                _defaultSession.Dispose();
            });
        }
    }

    public class CustomerFake : ICustomer
    {
        public string Id { get; set; }
    }
}
