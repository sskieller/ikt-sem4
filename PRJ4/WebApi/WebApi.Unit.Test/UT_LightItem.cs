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
        private string Command;
        private string wakeup;
        private string sleep;
        private DateTime WakeUpTime;
        private DateTime SleepTime;

        

        [SetUp]
        public void SetUp()
        {
            _uut = new LightItem();

            string command = "Whatcommand";
            DateTime wakeUpTime = DateTime.Parse("2018-04-12 08:40:35");
            DateTime sleepTime = DateTime.Parse("2018-04-12 22:40:35");
            _uut.IsOn = true;
            _uut.IsRun = true;
        }

        [Test]
        public void LightItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Command, Is.EqualTo(Command));
            Assert.That(_uut.WakeUpTime, Is.EqualTo(WakeUpTime));
            Assert.That(_uut.SleepTime, Is.EqualTo(SleepTime));
            Assert.That(_uut.IsRun, Is.EqualTo(true));
            Assert.That(_uut.IsOn, Is.EqualTo(true));
        }
    }
}
