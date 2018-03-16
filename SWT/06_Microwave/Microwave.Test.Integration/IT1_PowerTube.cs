using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using NUnit.Framework;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_PowerTube
    {
        private IOutput _output;
        private PowerTube _uut;
        private StringWriter _sw;

        [SetUp]
        public void SetUp()
        {
            _output = new Output();
            _sw = new StringWriter();
            Console.SetOut(_sw);

            _uut = new PowerTube(_output);
        }

        [TestCase(50, "PowerTube works with 50 W\r\n", TestName = "Powerlevel 50W")]
        [TestCase(700, "PowerTube works with 700 W\r\n", TestName = "Powerlevel 700W")]
        public void TurnOn_PowerLevelOK_Allowed(int power, string expectedOutput)
        {
            _uut.TurnOn(power);

            Assert.That(_sw.ToString(), Is.EqualTo(expectedOutput));
        }

        [TestCase(49, TestName = "Powerlevel 49W")]
        [TestCase(701, TestName = "Powerlevel 701W")]
        public void TurnOn_PowerLevelOutOfRange_NotAllowed(int power)
        {
            Assert.That(() => _uut.TurnOn(power), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(100, "PowerTube works with 100 W\r\n", TestName = "Powerlevel 100W NotOn")]
        public void TurnOn_PowerlevelOK_IsNotOn(int power, string expectedOutput)
        {
            _uut.TurnOn(power);
            Assert.That(() => _sw.ToString(), Is.EqualTo(expectedOutput));
        }

        [TestCase(100, TestName = "AlreadyOn, Powerlevel 100")]
        public void TurnOn_IsAlreadyOn_NotAllowed(int power)
        {
            _uut.TurnOn(power);
            Assert.That(() => _uut.TurnOn(power), Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public void TurnOff_IsAlreadyOn_Allowed()
        {
            _uut.TurnOn(100);
            _sw.GetStringBuilder().Clear();
            _uut.TurnOff();
            Assert.That(() => _sw.ToString(), Is.EqualTo("PowerTube turned off\r\n"));
        }
    }
}