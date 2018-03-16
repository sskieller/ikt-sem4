using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT2_Display
    {
        private IOutput _output;
        private StringWriter _sw;
        private IDisplay _display;

        [SetUp]
        public void SetUp()
        {
            // Setting up output and redirecting console output
            _output = new Output();
            _sw = new StringWriter();
            Console.SetOut(_sw);

            // Setting up display
            _display = new Display(_output);
        }

        [TestCase(0, 1, "Display shows: 00:01\r\n", TestName = "ShowTime - 0min, 1sec")]
        [TestCase(10, 1, "Display shows: 10:01\r\n", TestName = "ShowTime - 10min, 1sec")]
        [TestCase(2, 33, "Display shows: 02:33\r\n", TestName = "ShowTime - 2min, 33sec")]
        public void ShowTime_Test(int min, int sec, string expectedOutput)
        {
            _display.ShowTime(min, sec);
            Assert.That(_sw.ToString(), Is.EqualTo(expectedOutput));
        }

        [TestCase(50, "Display shows: 50 W\r\n", TestName = "Test Power = 50")]
        [TestCase(700, "Display shows: 700 W\r\n", TestName = "Test Power = 700")]
        public void ShowPower_Test(int power, string expectedOutput)
        {
            _display.ShowPower(power);
            Assert.That(_sw.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void ClearDisplay_Test()
        {
            string expectedOutput = "Display cleared\r\n";

            _display.Clear();

            Assert.That(_sw.ToString(), Is.EqualTo(expectedOutput));
        }
    }
}
