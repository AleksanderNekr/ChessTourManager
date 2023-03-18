using ChessTourManager.DataAccess;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.Domain.Algorithms;

namespace ChessTourManager.UnitTests
{
    public class RoundRobinTests
    {
        private          IRoundRobin      _roundRobin;
        private readonly ChessTourContext _context = ChessTourContext.CreateInstance();
        private          Tournament       _tournament;

        [SetUp]
        public void Setup()
        {
        }

        #region First tour test

        [Test]
        public void Test1()
        {
            _roundRobin = IRoundRobin.Initialize(_context, _tournament);
            Assert.Pass();
        }

        #endregion
    }
}
