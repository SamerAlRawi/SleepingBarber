using System;
using NSubstitute;
using NUnit.Framework;

namespace SleepingBarber.Tests
{
    [TestFixture]
    public class BarberLogManagerTests
    {
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            BarberLogManager.Logger = _logger;
        }

        [Test]
        public void Wraps_Logger_with_LoggerWrapper()
        {
            Assert.IsInstanceOf<LoggerWrapper>(BarberLogManager.Logger);
        }

        [Test]
        public void Handles_Exceptions()
        {
            var barberException = new BarberException();
            var exceptionLogged = false;
            
            _logger.When(l=>l.LogException(barberException)).Do(_ =>
            {
                exceptionLogged = true;
                throw new Exception();
            });
            
            Assert.DoesNotThrow(()=>BarberLogManager.Logger.LogException(barberException));
            Assert.IsTrue(exceptionLogged);
        }
    }
}
