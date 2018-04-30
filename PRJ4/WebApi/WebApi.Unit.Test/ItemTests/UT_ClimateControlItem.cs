using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_ClimateControlItem
    {
        private ClimateControlItem _uut;
        private string _command;
        private bool _isRun;
        private int _temperatureLevel;
        private int _minTemperature;
        private int _maxTemperature;
        private int _humidityLevel;
        private int _maxHumidity;
        private int _minHumidity;
        private bool _isVentilationOn;
        private bool _isHeaterOn;

        [SetUp]
        public void SetUp()
        {
            _command = "WhatCommand";
            _isRun = true;
            _temperatureLevel = 42;
            _minTemperature = 35;
            _maxTemperature = 45;
            _humidityLevel = 75;
            _minHumidity = 60;
            _maxHumidity = 85;
            _isVentilationOn = true;
            _isHeaterOn = false;

            _uut = new ClimateControlItem()
            {
                Command = _command,
                IsRun = _isRun,
                TemperatureLevel = _temperatureLevel,
                MinTemperature = _minTemperature,
                MaxTemperature = _maxTemperature,
                HumidityLevel = _humidityLevel,
                MinHumidity = _minHumidity,
                MaxHumidity = _maxHumidity,
                IsVentilationOn = _isVentilationOn,
                IsHeaterOn = _isHeaterOn
            };
        }

        [Test]
        public void ClimateControlItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Command, Is.EqualTo(_command));
            Assert.That(_uut.IsRun, Is.EqualTo(_isRun));
            Assert.That(_uut.TemperatureLevel, Is.EqualTo(_temperatureLevel));
            Assert.That(_uut.MinTemperature, Is.EqualTo(_minTemperature));
            Assert.That(_uut.MaxTemperature, Is.EqualTo(_maxTemperature));
            Assert.That(_uut.HumidityLevel, Is.EqualTo(_humidityLevel));
            Assert.That(_uut.MinHumidity, Is.EqualTo(_minHumidity));
            Assert.That(_uut.MaxHumidity, Is.EqualTo(_maxHumidity));
            Assert.That(_uut.IsVentilationOn, Is.EqualTo(_isVentilationOn));
            Assert.That(_uut.IsHeaterOn, Is.EqualTo(_isHeaterOn));
        }
    }
}
