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

        [TestCase(1)]
        [TestCase(100, "PowerTube works with 100 %", TestName = "Powerlevel 100")]
        public void TurnOn_PowerLevelOK_Allowed(int number, string expectedOutput)
        {
            _uut.TurnOn(number);

            Assert.That(_sw.ToString(), Is.EqualTo(expectedOutput));
        }
    }
}
