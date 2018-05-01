using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_IpItem
    {
        private IpItem _uut;

        private string _ip;

        [SetUp]
        public void SetUp()
        {
         
            _ip = "123456789";

            _uut.Ip = _ip;
        }

        [Test]
        public void IpItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Ip, Is.EqualTo(_ip));
        }
    }
}
