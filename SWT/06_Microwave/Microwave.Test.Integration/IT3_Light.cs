using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
	[TestFixture]
    public class IT3_Light
	{
		private StringWriter _sw;
		private IOutput _output;
		private ILight _light;

		[SetUp]
		public void SetUp()
		{
			_sw = new StringWriter();
			_output = new Output();
			_light = new Light(_output);
			Console.SetOut(_sw);
		}

		[Test]
		public void TurnOn_Called_OutputCorrect()
		{
			_light.TurnOn();

			string expected = $"Light is turned on{Environment.NewLine}";
			Assert.AreEqual(expected, _sw.ToString());
		}
		[Test]
		public void TurnOnAndOff_Called_OutputCorrect()
		{
			_light.TurnOn();
			StringBuilder sb = _sw.GetStringBuilder();
			sb.Remove(0, sb.Length); //Clear console output

			_light.TurnOff();

			string expected = $"Light is turned off{Environment.NewLine}";
			Assert.AreEqual(expected, _sw.ToString());
		}
    }
}
