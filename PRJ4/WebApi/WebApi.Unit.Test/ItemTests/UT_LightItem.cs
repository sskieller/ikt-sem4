using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FWPS.Models;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_LightItem
    {
        private LightItem _uut;
        private string _command;
        private DateTime _wakeUpTime;
        private DateTime _sleepTime;
        private bool _isOn;
        private bool _isRun;
        

        [SetUp]
        public void SetUp()
        {
            _command = "Whatcommand";
            _wakeUpTime = DateTime.Parse("2018-04-12 08:40:35");
            _sleepTime = DateTime.Parse("2018-04-12 22:40:35");
            _isOn = true;
            _isRun = false;

            _uut = new LightItem()
            {
                Command = _command,
                WakeUpTime = _wakeUpTime,
                SleepTime = _sleepTime,
                IsOn = _isOn,
                IsRun = _isRun
            };
        }

        [Test]
        public void LightItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Command, Is.EqualTo(_command));
            Assert.That(_uut.WakeUpTime, Is.EqualTo(_wakeUpTime));
            Assert.That(_uut.SleepTime, Is.EqualTo(_sleepTime));
            Assert.That(_uut.IsRun, Is.EqualTo(_isRun));
            Assert.That(_uut.IsOn, Is.EqualTo(_isOn));
        }
    }
}
