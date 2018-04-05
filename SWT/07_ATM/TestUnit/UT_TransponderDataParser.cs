using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ATM;

namespace TestUnit
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
            Assert.That(wat, Is.EqualTo(1));
        }
    }
}
