using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace WebApi.Unit.Test
{
    [TestFixture()]
    class UT_SnapBoxItem
    {
        private SnapBoxItem _uut;
        private string _snapBoxId;
        private string _powerLevel;
        private bool _mailReceived;
        private string _receiverEmail;
        private string _checkSum;

        [SetUp]
        public void SetUp()
        {
            _snapBoxId = "007";
            _powerLevel = "9001";
            _mailReceived = true;
            _receiverEmail = "BOB";
            _checkSum = "902309jifje091nd2d@@]£";

            _uut = new SnapBoxItem()
            {
                Checksum = _checkSum,
                MailReceived = _mailReceived,
                PowerLevel = _powerLevel,
                ReceiverEmail = _receiverEmail,
                SnapBoxId = _snapBoxId,
            };
        }

        [Test]
        public void SnapBoxItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Checksum, Is.EqualTo(_checkSum));
            Assert.That(_uut.MailReceived, Is.EqualTo(_mailReceived));
            Assert.That(_uut.PowerLevel, Is.EqualTo(_powerLevel));
            Assert.That(_uut.ReceiverEmail, Is.EqualTo(_receiverEmail));
            Assert.That(_uut.SnapBoxId, Is.EqualTo(_snapBoxId));
        }
    }
}
