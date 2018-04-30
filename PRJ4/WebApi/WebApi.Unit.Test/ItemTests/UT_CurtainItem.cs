using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_CurtainItem
    {
        private CurtainItem _uut;

        private string _command;
        private bool _isRun;
        private int _maxLightIntensity;
        private int _lightIntensity;
        private string _status;

        [SetUp]
        public void SetUp()
        {
            _command = "WhatCommand";
            _isRun = true;
            _maxLightIntensity = 42;
            _lightIntensity = 30;
            _status = "Not a duck";

            _uut = new CurtainItem()
            {
                Command = _command,
                IsRun = _isRun,
                MaxLightIntensity = _maxLightIntensity,
                LightIntensity = _lightIntensity,
                Status = _status
            };
        }

        [Test]
        public void Curtainitem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Command, Is.EqualTo(_command));
            Assert.That(_uut.IsRun, Is.EqualTo(_isRun));
            Assert.That(_uut.MaxLightIntensity, Is.EqualTo(_maxLightIntensity));
            Assert.That(_uut.LightIntensity, Is.EqualTo(_lightIntensity));
            Assert.That(_uut.Status, Is.EqualTo(_status));
        }
    }
}
