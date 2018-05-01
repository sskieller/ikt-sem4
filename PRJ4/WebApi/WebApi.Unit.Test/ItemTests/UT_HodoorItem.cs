using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_HodoorItem
    {
        private HodoorItem _uut;

        private string _command;
        private bool _openStatus;
        private bool _isRun;


        [SetUp]
        public void SetUp()
        {
            _command = "What";
            _openStatus = true;
            _isRun = false;

            _uut = new HodoorItem()
            {
                Command = _command,
                OpenStatus = _openStatus,
                IsRun = _isRun
            };
        }

        [Test]
        public void HodoorItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Command, Is.EqualTo(_command));
            Assert.That(_uut.OpenStatus, Is.EqualTo(_openStatus));
            Assert.That(_uut.IsRun, Is.EqualTo(_isRun));
        }
    }
}
