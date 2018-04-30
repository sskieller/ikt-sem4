using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_MailItem
    {
        private MailItem _uut;
        private string _from;
        private string _to;
        private string _subject;
        private string _body;

        [SetUp]
        public void SetUp()
        {
            _from = "Bob";
            _to = "Benny";
            _subject = "doors";
            _body = "No dooors here lol";

            _uut = new MailItem()
            {
                From = _from,
                To = _to,
                Subject = _subject,
                Body = _body
            };
        }

        [Test]
        public void MailItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.From, Is.EqualTo(_from));
            Assert.That(_uut.To, Is.EqualTo(_to));
            Assert.That(_uut.Subject, Is.EqualTo(_subject));
            Assert.That(_uut.Body, Is.EqualTo(_body));
        }
    }
}
