using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Extensions;

namespace Microwave.Test.Integration
{
	[TestFixture]
	class IT4_UI
	{
		private StringWriter _sw;
		private UserInterface _ui;
		private IDisplay _display;
		private ILight _light;
		private IButton _powerButton;
		private IButton _timeButton;
		private IButton _startCancelButton;
		private IDoor _door;
		private ICookController _cookController;

		[SetUp]
		public void SetUp()
		{
			_sw = new StringWriter();
			_powerButton = new Button();
			_timeButton = new Button();
			_startCancelButton = new Button();
			_door = new Door();
			_cookController = Substitute.For<ICookController>();
			_light = new Light(new Output());
			_display = new Display(new Output());
			_ui = new UserInterface(_powerButton, _timeButton, _startCancelButton,
				_door, _display, _light, _cookController);
			Console.SetOut(_sw);
		}

		[Test]
		public void OnPowerPressed_ReadyState_ShowPowerExecuted()
		{
			_powerButton.Press();


			string expected = $"Display shows: 50 W{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
			
		}
		[Test]
		public void OnPowerPressed_SetPowerStateClickedThrice_ShowPowerlevelCorrectly()
		{
			_powerButton.Press();
			_powerButton.Press();
			_powerButton.Press();
			_sw.GetStringBuilder().Clear();

			_powerButton.Press();
			string expected = $"Display shows: 200 W{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}
		[Test]
		public void OnPowerPressed_SetPowerStateClickedOnce_ShowPowerlevelCorrectly()
		{
			_powerButton.Press();
			ClearConsole();

			_powerButton.Press();
			string expected = $"Display shows: 100 W{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());

		}

		[Test]
		public void OnPowerPressed_SetPowerStateClickedTenTimes_ShowPeterLevelReset()
		{
			for (int i = 0; i < 14; ++i)
				_powerButton.Press();
			ClearConsole();

			_powerButton.Press();
			string expected = $"Display shows: 50 W{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());

		}

		[Test]
		public void OnTimePressed_SetPower_ShowOneMinute()
		{
			_powerButton.Press();
			ClearConsole();
			_timeButton.Press();

			int min = 1, sec = 0;

			string expected = $"Display shows: {min:D2}:{sec:D2}{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}

		[Test]
		public void OnTimePressed_SetTimePressedTwice_ShowThreeMinutes()
		{
			_powerButton.Press();
			_timeButton.Press();
			_timeButton.Press();
			ClearConsole();
			_timeButton.Press();
			

			int min = 3, sec = 0;

			string expected = $"Display shows: {min:D2}:{sec:D2}{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}
		[Test]
		public void OnStartCancelPressed_SetPower_LightOffDisplayCleared()
		{
			_powerButton.Press();
			ClearConsole();
			_startCancelButton.Press();


			string expected = $"Display cleared{Environment.NewLine}";
			                  

			Assert.AreEqual(expected, _sw.ToString());
		}
		[Test]
		public void OnStartCancelPressed_SetTime_LightOnDisplayCleared()
		{
			_powerButton.Press();
			_timeButton.Press();
			ClearConsole();
			_startCancelButton.Press();


			string expected = $"Display cleared{Environment.NewLine}" +
			                  $"Light is turned on{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}

		[Test]
		public void OnStartCancelPressed_Cooking_LightOffDisplayClear()
		{
			_powerButton.Press();
			_timeButton.Press();
			_startCancelButton.Press();
			ClearConsole();
			_startCancelButton.Press();


			string expected = $"Light is turned off{Environment.NewLine}"
				+ $"Display cleared{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}

		[Test]
		public void OnDoorOpened_OpenDoorReady_LightTurnedOn()
		{
			_door.Open();

			string expected = $"Light is turned on{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());

		}

		[Test]
		public void OnDoorOpened_OpenDoorSetPower_DisplayCleared()
		{
			_powerButton.Press();
			ClearConsole();

			_door.Open();

			string expected = $"Light is turned on{Environment.NewLine}" + 
				$"Display cleared{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}

		[Test]
		public void OnDoorOpened_OpenDoorSettime_DisplayCleared()
		{
			_powerButton.Press();
			_timeButton.Press();
			ClearConsole();

			_door.Open();

			string expected = $"Light is turned on{Environment.NewLine}" +
			                  $"Display cleared{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}

		[Test]
		public void OnDoorClosed_CloseDoorWhenDoorOpen_LightOff()
		{
			_door.Open();
			ClearConsole();
			_door.Close();

			string expected = $"Light is turned off{Environment.NewLine}";

			Assert.AreEqual(expected, _sw.ToString());
		}

		[Test]
		public void CookingIsDone_Called_ClearDisplayAndLight()
		{
			
			_powerButton.Press();
			_timeButton.Press();
			_startCancelButton.Press();
			_cookController.StartCooking(Arg.Any<int>(), Arg.Any<int>());
			
			ClearConsole();
			_ui.CookingIsDone();


			string expected = $"Display cleared{Environment.NewLine}" 
			                  + $"Light is turned off{Environment.NewLine}";
			                  ;

			Assert.AreEqual(expected, _sw.ToString());
		}



		private void ClearConsole()
		{
			StringBuilder sb = _sw.GetStringBuilder();
			sb.Remove(0, sb.Length);
		}
	}
}
