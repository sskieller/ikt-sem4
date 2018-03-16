using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Threading;

namespace Microwave.Test.Integration
{
    [TestFixture]
	class IT5_CookController
    {
        private ICookController _cookController;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private IUserInterface _userInterface;
        private IOutput _output;


	    [SetUp]
	    public void SetUp()
	    {
            _output = Substitute.For<IOutput>();
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);

	        _userInterface = Substitute.For<IUserInterface>();

            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
	    }

        [Test]
        public void StartCooking_Test_PowerTube_On()
        {
            _cookController.StartCooking(50, 50);

            _output.Received(1).OutputLine(Arg.Is<string>(str => 
                str.Contains("PowerTube works with 50 W")));
            
        }

        [Test]
        public void StartCooking_Test_PowerTube_Off()
        {
            _cookController.StartCooking(50, 50);

            Thread.Sleep(200);

            _cookController.Stop();

            _output.Received(1).OutputLine(Arg.Is<string>(str =>
                str.Contains("PowerTube turned off")));
        }

        [Test]
        public void StartCooking_Test_Timer_No_tick()
        {
            _cookController.StartCooking(50, 50);

            Thread.Sleep(800);

            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => 
                str.Contains("Display shows:")));
        }

        [Test]
        public void StartCooking_Test_Timer_And_Display_One_tick()
        {
            _cookController.StartCooking(50, 50);

            Thread.Sleep(1200);

            _output.Received().OutputLine("PowerTube works with 50 W");
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display shows:")));
        }

        [Test]
        public void StartCooking_Test_Timer_And_Display_Two_tick()
        {
            _cookController.StartCooking(50, 50);

            Thread.Sleep(2200);

            _output.Received().OutputLine("PowerTube works with 50 W");
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 00:49")));
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 00:48")));

        }

        [Test]
        public void StartCooking_Test_Timer_No_Ticks_After_Stop()
        {
            _cookController.StartCooking(50, 50);

            Thread.Sleep(1100);

            _output.Received().OutputLine("PowerTube works with 50 W");
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 00:49")));

            // Stopping and clearing
            _cookController.Stop();
            _output.ClearReceivedCalls();

            Thread.Sleep(1100);

            _output.DidNotReceiveWithAnyArgs().OutputLine(" ");
        }

        [Test]
        public void StartCooking_Test_UI_CookingIsDone_Not_Called()
        {
            _cookController.StartCooking(50, 50);

            _userInterface.DidNotReceive().CookingIsDone();
        }

        [Test]
        public void StartCooking_Test_UI_CookingIsDone_Called()
        {
            _cookController.StartCooking(50, 2);
            
            Thread.Sleep(2200);

            _userInterface.Received().CookingIsDone();
        }

    }
}
