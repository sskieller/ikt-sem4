using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_LoginItem
    {
        private LoginItem _uut;

        private string _username;
        private string _password;

        [SetUp]
        public void SetUp()
        {
            _username = "bob";
            _password = "asd809admxnx0123m123o";

            _uut = new LoginItem()
            {
                Username = _username,
                Password = _password
            };
        }

        [Test]
        public void LoginItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Username, Is.EqualTo(_username));
            Assert.That(_uut.Password, Is.EqualTo(_password));
        }
    }
}
