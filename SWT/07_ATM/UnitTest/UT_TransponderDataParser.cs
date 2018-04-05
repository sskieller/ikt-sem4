using System;
using ATM;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class UT_TransponderDataParser
    {

        private ITransponderDataParser _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new TransponderDataParser();
        }

        [Test]
        public void FirstTest()
        {
            int wat = 1;
            Assert.That(wat,Is.EqualTo(1));
        }
    }
}
