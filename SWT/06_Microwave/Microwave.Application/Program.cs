using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Application
{
	class Program
	{
		static void Main(string[] args)
		{
            // Output
            Output output = new Output();
            // Display
		    IDisplay display = new Display(output);
            // CookController
            IPowerTube powerTube = new PowerTube(output);
		    ITimer timer = new Timer();
            ICookController cookController =
                new CookController(timer, display, powerTube);
            // UserInterface
		    IButton powerButton = new Button();
            IButton timerButton = new Button();
            IButton startCancelButton = new Button();
		    IDoor door = new Door();
		    ILight light = new Light(output);
		    IUserInterface ui = new UserInterface(powerButton, timerButton, startCancelButton,
		        door, display, light, cookController);

		    cookController.UI = ui;

		    Console.BackgroundColor = ConsoleColor.Cyan;
		    Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Main UC");

            // UC 1
            door.Open();
            // UC 4
		    door.Close();

		    RunUC6ToEnd(powerButton, timerButton, startCancelButton, timer, door);


            Console.WriteLine("Ext1 UC6");
            // Extension 1 - Start from UC6
            door.Open();
            door.Close();

            powerButton.Press();
            startCancelButton.Press();

		    RunUC6ToEnd(powerButton, timerButton, startCancelButton, timer, door);

            Console.WriteLine("Ext1 UC15");
            // Extension 1 - Start from UC15
            door.Open();
            door.Close();

            powerButton.Press();
            startCancelButton.Press();

            RunUC15ToEnd(door);

            Console.WriteLine("Ext2 Powerbutton: UC4");
            // Extension 2 - Power button: Start from UC4
            door.Open();
            door.Close();

            powerButton.Press();
            door.Open();
		    RunUC4ToEnd(powerButton, timerButton, startCancelButton, timer, door);

            Console.WriteLine("Ext2 Powerbutton: UC17");
            // Extension 2 - Power button: Start from UC17
            door.Open();
            door.Close();
            
            powerButton.Press();
            door.Open();
            RunUC17ToEnd(door);

            Console.WriteLine("Ext2 Timerbutton: UC4");
		    // Extension 2 - Timer button: Start from UC4
            door.Open();
            door.Close();

            powerButton.Press();
            timerButton.Press();

		    door.Open();
		    RunUC4ToEnd(powerButton, timerButton, startCancelButton, timer, door);

            Console.WriteLine("Ext2 Timerbutton: UC17");
		    // Extension 2 - Timer button: Start from UC17
            door.Open();
            door.Close();

            powerButton.Press();
            timerButton.Press();

            door.Open();
            RunUC17ToEnd(door);

            Console.WriteLine("Ext3 UC6");
            // Extension 3 - Start from UC6
            door.Open();
            door.Close();

            powerButton.Press();
            timerButton.Press();

            startCancelButton.Press();
            // Before timer ends
		    Thread.Sleep(200);
            startCancelButton.Press();

		    RunUC6ToEnd(powerButton, timerButton, startCancelButton, timer, door);

            Console.WriteLine("Ext3 UC15");
            // Extension 3 - Start from UC15
		    door.Open();
		    door.Close();

		    powerButton.Press();
		    timerButton.Press();

		    startCancelButton.Press();
            // Before timer ends
		    Thread.Sleep(200);
            startCancelButton.Press();

            RunUC15ToEnd(door);

            Console.WriteLine("Ext4 UC4");
            // Extension 4 - Start from UC4
		    door.Open();
		    door.Close();

		    powerButton.Press();
		    timerButton.Press();

		    startCancelButton.Press();
            // Before timer ends
		    Thread.Sleep(200);
            door.Open();

		    RunUC4ToEnd(powerButton, timerButton, startCancelButton, timer, door);

            Console.WriteLine("Ext4 UC17");
            // Extension 4 - Start from UC17
		    door.Open();
		    door.Close();

		    powerButton.Press();
		    timerButton.Press();

		    startCancelButton.Press();

		    // Before timer ends
            Thread.Sleep(200);
		    door.Open();

            RunUC17ToEnd(door);

		    Console.ReadKey();
		}

        private static void RunUC4ToEnd(IButton powerButton,
	        IButton timerButton,
	        IButton startCancelButton,
	        ITimer timer,
	        IDoor door)
	    {
	        // UC 4
	        door.Close();
	        // UC 6 to End
	        RunUC6ToEnd(powerButton, timerButton, startCancelButton, timer, door);
	    }

        private static void RunUC6ToEnd(IButton powerButton, 
	        IButton timerButton, 
	        IButton startCancelButton, 
	        ITimer timer,
	        IDoor door)
	    {
	        // UC 6, Powerlevel 200
	        for (int i = 0; i < 3; i++)
	        {
	            powerButton.Press();
	        }
	        // UC 7, Timer set to 1 minute
	        for (int i = 0; i < 1; i++)
	        {
	            timerButton.Press();
	        }

	        // UC 8
	        startCancelButton.Press();

	        // UC 9-14
	        while (timer.TimeRemaining > 0)
	        {
	            // Do nothing
	        }

            Thread.Sleep(200);
	        // UC 15
	        door.Open();
	        // UC 17 to End
	        RunUC17ToEnd(door);
        }

	    private static void RunUC15ToEnd(IDoor door)
	    {
            // UC 15
            door.Open();
            // UC 17 
            RunUC17ToEnd(door);
	    }

	    

	    private static void RunUC17ToEnd(IDoor door)
	    {
            // UC 17
            // Removes food
            // UC 18
            door.Close();
	    }
	}
}
